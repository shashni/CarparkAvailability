"""Client for the OutSystems LifeTime Deployment API v2.

Endpoints used (OutSystems 11 documentation, "LifeTime API v2" and its examples):
  GET  /environments/
  GET  /applications/
  GET  /environments/{EnvironmentKey}/applications/{ApplicationKey}/?IncludeEnvStatus=true&IncludeModules=true
  POST /environments/{EnvironmentKey}/applications/{ApplicationKey}/versions/
  POST /deployments/
  GET  /deployments/{DeploymentKey}/
  POST /deployments/{DeploymentKey}/{start|continue|abort}/
  GET  /deployments/{DeploymentKey}/status/
  DELETE /deployments/{DeploymentKey}/
Authentication: service account token sent as "Authorization: Bearer <token>".
The API has no site-property operations; those are set in Service Center and verified through the app's
health endpoint (see carpark_ops.health).
"""

from __future__ import annotations

import time
from dataclasses import dataclass
from typing import Any

import requests

API_PATH = "/lifetimeapi/rest/v2"
TERMINAL_STATUSES = frozenset(
    {"finished_successful", "finished_with_warnings", "finished_with_errors", "aborted", "needs_user_intervention"}
)
SUCCESS_STATUSES = frozenset({"finished_successful", "finished_with_warnings"})
COMMANDS = frozenset({"start", "continue", "abort"})


class LifeTimeError(Exception):
    def __init__(self, message: str, status: int | None = None, body: str = ""):
        super().__init__(message)
        self.status = status
        self.body = body


@dataclass
class DeploymentStatus:
    status: str
    info: str
    log: list[dict[str, Any]]

    @property
    def is_terminal(self) -> bool:
        return self.status in TERMINAL_STATUSES

    @property
    def succeeded(self) -> bool:
        return self.status in SUCCESS_STATUSES


class LifeTimeClient:
    def __init__(
        self,
        base_url: str,
        token: str,
        *,
        session: requests.Session | None = None,
        timeout: float = 60.0,
        retries: int = 3,
        backoff_seconds: float = 2.0,
        sleep=time.sleep,
    ):
        if not base_url.startswith("https://"):
            raise ValueError("LifeTime URL must use https")
        if not token:
            raise ValueError("A LifeTime service account token is required")
        self.root = base_url.rstrip("/").removesuffix("/lifetime") + API_PATH
        self.session = session or requests.Session()
        self.session.headers.update({"Authorization": f"Bearer {token}", "Accept": "application/json"})
        self.timeout = timeout
        self.retries = retries
        self.backoff_seconds = backoff_seconds
        self._sleep = sleep

    # ---- transport -------------------------------------------------------------------------------------
    def _request(self, method: str, path: str, *, params=None, json=None, expect_json: bool = True) -> Any:
        url = self.root + path
        last: Exception | None = None
        for attempt in range(self.retries + 1):
            try:
                resp = self.session.request(method, url, params=params, json=json, timeout=self.timeout)
            except requests.RequestException as exc:  # network problems are retried
                last = exc
            else:
                if resp.status_code in (429, 502, 503, 504) and attempt < self.retries:
                    last = LifeTimeError(f"{method} {path} -> HTTP {resp.status_code}", resp.status_code, resp.text)
                elif resp.status_code >= 400:
                    raise LifeTimeError(
                        f"{method} {path} -> HTTP {resp.status_code}: {resp.text[:500]}", resp.status_code, resp.text
                    )
                else:
                    if not expect_json or not resp.content:
                        return None
                    return resp.json()
            if attempt < self.retries:
                self._sleep(self.backoff_seconds * (2**attempt))
        raise LifeTimeError(f"{method} {path} failed after {self.retries + 1} attempts: {last}")

    # ---- read ------------------------------------------------------------------------------------------
    def list_environments(self) -> list[dict[str, Any]]:
        return self._request("GET", "/environments/")

    def list_applications(self) -> list[dict[str, Any]]:
        return self._request("GET", "/applications/", params={"IncludeEnvStatus": "true"})

    def get_application(self, environment_key: str, application_key: str) -> dict[str, Any]:
        return self._request(
            "GET",
            f"/environments/{environment_key}/applications/{application_key}/",
            params={"IncludeEnvStatus": "true", "IncludeModules": "true"},
        )

    def environment_key(self, name: str) -> str:
        envs = self.list_environments()
        for env in envs:
            if env.get("Name", "").lower() == name.lower():
                return env["Key"]
        raise LifeTimeError(f"LifeTime has no environment named '{name}' (found: {[e.get('Name') for e in envs]})")

    def application_key(self, name: str) -> str:
        apps = self.list_applications()
        for app in apps:
            if app.get("Name") == name:
                return app["Key"]
        raise LifeTimeError(f"LifeTime has no application named '{name}'")

    # ---- write -----------------------------------------------------------------------------------------
    def create_version(
        self, environment_key: str, application_key: str, version: str, change_log: str, module_version_keys: list[str]
    ) -> str:
        body = {
            "ChangeLog": change_log,
            "Version": version,
            "MobileVersions": [],
            "ModuleVersionKeys": module_version_keys,
        }
        result = self._request(
            "POST", f"/environments/{environment_key}/applications/{application_key}/versions/", json=body
        )
        return result["ApplicationVersionKey"]

    def create_deployment(
        self, source_key: str, target_key: str, application_version_keys: list[str], notes: str
    ) -> str:
        body = {
            "Notes": notes,
            "SourceEnvironmentKey": source_key,
            "TargetEnvironmentKey": target_key,
            "ApplicationOperations": [
                {"ApplicationVersionKey": key, "DeploymentZoneKey": ""} for key in application_version_keys
            ],
        }
        result = self._request("POST", "/deployments/", json=body)
        return result if isinstance(result, str) else result.get("Key") or result.get("DeploymentKey")

    def get_deployment(self, deployment_key: str) -> dict[str, Any]:
        return self._request("GET", f"/deployments/{deployment_key}/")

    def command(self, deployment_key: str, command: str) -> None:
        if command not in COMMANDS:
            raise ValueError(f"Unsupported deployment command {command!r}")
        self._request("POST", f"/deployments/{deployment_key}/{command}/", expect_json=False)

    def delete_deployment(self, deployment_key: str) -> None:
        self._request("DELETE", f"/deployments/{deployment_key}/", expect_json=False)

    def status(self, deployment_key: str) -> DeploymentStatus:
        doc = self._request("GET", f"/deployments/{deployment_key}/status/")
        return DeploymentStatus(doc.get("DeploymentStatus", ""), doc.get("Info", ""), doc.get("DeploymentLog", []))

    def wait(
        self, deployment_key: str, *, poll_seconds: float, timeout_seconds: float, on_poll=None
    ) -> DeploymentStatus:
        deadline = time.monotonic() + timeout_seconds
        while True:
            current = self.status(deployment_key)
            if on_poll:
                on_poll(current)
            if current.is_terminal:
                return current
            if time.monotonic() >= deadline:
                raise LifeTimeError(
                    f"Deployment {deployment_key} still '{current.status}' after {timeout_seconds:.0f}s"
                )
            self._sleep(poll_seconds)


def env_status(application: dict[str, Any], environment_key: str) -> dict[str, Any]:
    """The AppStatusInEnvs entry for one environment (empty dict when the app is not there)."""
    for entry in application.get("AppStatusInEnvs") or []:
        if entry.get("EnvironmentKey") == environment_key:
            return entry
    return {}


def module_version_keys(application: dict[str, Any], environment_key: str) -> list[str]:
    """Module version keys currently running in an environment (input for creating an application version)."""
    keys: list[str] = []
    for module in env_status(application, environment_key).get("ModuleStatusInEnvs") or []:
        key = module.get("ModuleVersionKey")
        if key:
            keys.append(key)
    if not keys:  # some LifeTime versions return modules at the top level
        for module in application.get("Modules") or []:
            for status in module.get("ModuleStatusInEnvs") or []:
                if status.get("EnvironmentKey") == environment_key and status.get("ModuleVersionKey"):
                    keys.append(status["ModuleVersionKey"])
    return keys
