"""Promotes the application between LifeTime environments according to infra/pipeline.json.

Flow (plan -> apply):
  1. Resolve environment and application keys by name.
  2. Decide what to deploy: when the stage has createVersion=true and the source is modified, tag a new
     application version from the modules running in the source; otherwise reuse the source's base version.
  3. Create the deployment, inspect it for conflicts (abort and delete it if any, unless allowed).
  4. Start it and poll the status until it reaches a terminal state.
Site properties are not part of a LifeTime deployment; after a successful deployment run
`carpark-ops drift --env <target>` (CI does this automatically) to confirm the target's configuration.
"""

from __future__ import annotations

import re
from dataclasses import dataclass, field
from typing import Any

from .config import Environment
from .lifetime import LifeTimeClient, LifeTimeError, env_status, module_version_keys

VERSION_PATTERN = re.compile(r"^\d+\.\d+(\.\d+)?$")


class DeploymentError(Exception):
    pass


@dataclass
class Plan:
    application: str
    source: str
    target: str
    source_key: str
    target_key: str
    application_key: str
    create_version: bool
    version: str | None
    base_version_key: str | None
    module_version_keys: list[str]
    source_modified: bool
    notes: str
    warnings: list[str] = field(default_factory=list)

    def describe(self) -> str:
        lines = [
            f"Application : {self.application} ({self.application_key})",
            f"Promotion   : {self.source} ({self.source_key}) -> {self.target} ({self.target_key})",
        ]
        if self.create_version:
            lines.append(f"Version     : create {self.version} from {len(self.module_version_keys)} running module(s)")
        else:
            lines.append(f"Version     : reuse base version {self.base_version_key}")
        lines.append(
            f"Source state: {'modified since last version' if self.source_modified else 'matches a tagged version'}"
        )
        lines += [f"Warning     : {w}" for w in self.warnings]
        return "\n".join(lines)


@dataclass
class Result:
    plan: Plan
    deployment_key: str | None
    status: str
    info: str = ""
    log: list[dict[str, Any]] = field(default_factory=list)

    @property
    def succeeded(self) -> bool:
        return self.status in ("finished_successful", "finished_with_warnings", "planned")


def find_stage(pipeline: dict[str, Any], source: str, target: str) -> dict[str, Any]:
    for stage in pipeline["stages"]:
        if stage["from"] == source and stage["to"] == target:
            return stage
    allowed = ", ".join(f"{s['from']}->{s['to']}" for s in pipeline["stages"])
    raise DeploymentError(f"{source}->{target} is not a pipeline stage (allowed: {allowed})")


def plan_promotion(
    client: LifeTimeClient,
    app: dict[str, Any],
    pipeline: dict[str, Any],
    source: Environment,
    target: Environment,
    *,
    version: str | None,
    notes: str,
) -> Plan:
    stage = find_stage(pipeline, source.name, target.name)
    for env in (source, target):
        if not env.is_lifetime:
            raise DeploymentError(f"'{env.name}' is not managed by LifeTime; publish it from Service Studio instead")
        if not env.enabled:
            raise DeploymentError(f"environment '{env.name}' is disabled in infra/environments/{env.name}.json")

    source_key = client.environment_key(source.lifetime_environment_name)
    target_key = client.environment_key(target.lifetime_environment_name)
    app_name = app["lifetimeApplicationName"]
    app_key = client.application_key(app_name)
    details = client.get_application(source_key, app_key)
    status = env_status(details, source_key)
    if not status:
        raise DeploymentError(f"{app_name} is not installed in {source.name}")
    modified = bool(status.get("IsModified"))
    base_version_key = status.get("BaseApplicationVersionKey")
    warnings: list[str] = []
    create = stage["createVersion"] and (modified or not base_version_key)

    if create:
        if not version or not VERSION_PATTERN.match(version):
            raise DeploymentError("this stage tags a new version: pass --version MAJOR.MINOR[.PATCH]")
        modules = module_version_keys(details, source_key)
        if not modules:
            raise DeploymentError(f"no module versions found for {app_name} in {source.name}")
    else:
        modules = []
        if modified:
            warnings.append(
                f"{source.name} has changes not in a tagged version; deploying the last tagged version "
                f"{base_version_key} (the untagged changes are NOT promoted)"
            )
        if not base_version_key:
            raise DeploymentError(f"{app_name} has no tagged version in {source.name}")
    if status.get("ConsistencyStatus") not in (None, "", "OK"):
        warnings.append(f"source consistency status is {status.get('ConsistencyStatus')}")

    return Plan(
        application=app_name,
        source=source.name,
        target=target.name,
        source_key=source_key,
        target_key=target_key,
        application_key=app_key,
        create_version=create,
        version=version if create else None,
        base_version_key=base_version_key,
        module_version_keys=modules,
        source_modified=modified,
        notes=notes,
        warnings=warnings,
    )


def apply_plan(
    client: LifeTimeClient,
    plan: Plan,
    pipeline: dict[str, Any],
    *,
    log=print,
) -> Result:
    settings = pipeline["deployment"]
    if plan.create_version:
        version_key = client.create_version(
            plan.source_key, plan.application_key, plan.version, plan.notes, plan.module_version_keys
        )
        log(f"Created application version {plan.version} ({version_key})")
    else:
        version_key = plan.base_version_key

    deployment_key = client.create_deployment(plan.source_key, plan.target_key, [version_key], plan.notes)
    log(f"Created deployment {deployment_key}")
    details = client.get_deployment(deployment_key)
    conflicts = details.get("ApplicationConflicts") or []
    if conflicts and not settings["allowConflicts"]:
        try:
            client.delete_deployment(deployment_key)
        except LifeTimeError as exc:  # keep the original reason visible
            log(f"Could not delete deployment {deployment_key}: {exc}")
        raise DeploymentError(f"deployment has {len(conflicts)} conflict(s) and was discarded: {conflicts}")
    redeploy = details.get("ApplicationsToRedeploy") or []
    if redeploy:
        log(f"LifeTime will also redeploy {len(redeploy)} dependent application(s)")

    client.command(deployment_key, "start")
    log("Deployment started")
    final = client.wait(
        deployment_key,
        poll_seconds=settings["pollIntervalSeconds"],
        timeout_seconds=settings["timeoutMinutes"] * 60,
        on_poll=lambda s: log(f"  status: {s.status}"),
    )
    if final.status == "needs_user_intervention":
        log("Deployment needs user intervention in LifeTime (e.g. a confirmation); it was left running.")
    return Result(plan, deployment_key, final.status, final.info, final.log)
