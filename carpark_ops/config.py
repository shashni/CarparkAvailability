"""Loads and validates the infrastructure definition in infra/ (application, environments, pipeline)."""

from __future__ import annotations

import json
import os
import re
from dataclasses import dataclass, field
from pathlib import Path
from typing import Any

from jsonschema import Draft202012Validator

PLACEHOLDER_MARKER = "REPLACE_ME"


class ConfigError(Exception):
    """Raised when the infrastructure definition cannot be loaded."""


@dataclass
class Issue:
    """A validation finding. Errors block deployment; warnings are reported only."""

    severity: str  # "error" | "warning"
    source: str
    message: str

    def __str__(self) -> str:
        return f"[{self.severity}] {self.source}: {self.message}"


@dataclass
class Environment:
    name: str
    kind: str
    enabled: bool
    base_url: str
    site_properties: dict[str, Any]
    secrets: dict[str, dict[str, Any]]
    checks: dict[str, Any]
    lifetime_environment_name: str | None = None
    description: str = ""
    raw: dict[str, Any] = field(default_factory=dict, repr=False)

    @property
    def is_lifetime(self) -> bool:
        return self.kind == "lifetime"

    def url(self, path: str) -> str:
        return self.base_url.rstrip("/") + path


def repo_root(start: Path | None = None) -> Path:
    """Finds the repository root (the folder that holds infra/application.json)."""
    override = os.environ.get("CARPARK_REPO_ROOT")
    if override:
        return Path(override).resolve()
    here = (start or Path(__file__)).resolve()
    for candidate in [here, *here.parents]:
        if (candidate / "infra" / "application.json").is_file():
            return candidate
    raise ConfigError("Could not find infra/application.json above " + str(here))


def _read_json(path: Path) -> dict[str, Any]:
    try:
        return json.loads(path.read_text(encoding="utf-8"))
    except FileNotFoundError as exc:
        raise ConfigError(f"Missing file: {path}") from exc
    except json.JSONDecodeError as exc:
        raise ConfigError(f"Invalid JSON in {path}: {exc}") from exc


def _schema_issues(doc: dict[str, Any], schema_path: Path, source: str) -> list[Issue]:
    validator = Draft202012Validator(_read_json(schema_path))
    return [
        Issue("error", source, f"{'/'.join(str(p) for p in err.absolute_path) or '<root>'}: {err.message}")
        for err in sorted(validator.iter_errors(doc), key=lambda e: list(e.absolute_path))
    ]


def load_application(root: Path | None = None) -> dict[str, Any]:
    root = root or repo_root()
    return _read_json(root / "infra" / "application.json")


def load_pipeline(root: Path | None = None) -> dict[str, Any]:
    root = root or repo_root()
    return _read_json(root / "infra" / "pipeline.json")


def environment_names(root: Path | None = None) -> list[str]:
    root = root or repo_root()
    return sorted(p.stem for p in (root / "infra" / "environments").glob("*.json") if not p.stem.endswith(".local"))


def load_environment(name: str, root: Path | None = None) -> Environment:
    root = root or repo_root()
    path = root / "infra" / "environments" / f"{name}.json"
    doc = _read_json(path)
    local = root / "infra" / "environments" / f"{name}.local.json"
    if local.is_file():  # git-ignored developer overrides, never secrets
        doc = {**doc, **_read_json(local)}
    return Environment(
        name=doc["name"],
        kind=doc["kind"],
        enabled=doc["enabled"],
        base_url=doc["baseUrl"],
        site_properties=doc.get("siteProperties", {}),
        secrets=doc.get("secrets", {}),
        checks=doc.get("checks", {}),
        lifetime_environment_name=doc.get("lifetimeEnvironmentName"),
        description=doc.get("description", ""),
        raw=doc,
    )


def _check_value(prop: str, spec: dict[str, Any], value: Any, source: str) -> list[Issue]:
    issues: list[Issue] = []
    kind = spec["type"]
    if kind == "integer" and (not isinstance(value, int) or isinstance(value, bool)):
        return [Issue("error", source, f"{prop} must be an integer, got {value!r}")]
    if kind == "decimal" and (not isinstance(value, (int, float)) or isinstance(value, bool)):
        return [Issue("error", source, f"{prop} must be a number, got {value!r}")]
    if kind == "text" and not isinstance(value, str):
        return [Issue("error", source, f"{prop} must be text, got {value!r}")]
    if kind == "boolean" and not isinstance(value, bool):
        return [Issue("error", source, f"{prop} must be true/false, got {value!r}")]
    if "minimum" in spec and value < spec["minimum"]:
        issues.append(Issue("error", source, f"{prop}={value} is below the minimum {spec['minimum']}"))
    if "maximum" in spec and value > spec["maximum"]:
        issues.append(Issue("error", source, f"{prop}={value} is above the maximum {spec['maximum']}"))
    if "pattern" in spec and not re.fullmatch(spec["pattern"], value):
        issues.append(Issue("error", source, f"{prop}={value!r} does not match {spec['pattern']}"))
    return issues


def semantic_issues(app: dict[str, Any], env: Environment) -> list[Issue]:
    """Rules JSON Schema cannot express: cross-field constraints, secrets hygiene, placeholders."""
    src = f"environments/{env.name}.json"
    issues: list[Issue] = []
    specs: dict[str, Any] = app["siteProperties"]
    props = env.site_properties

    for prop, spec in specs.items():
        if spec.get("secret"):
            if prop in props:
                issues.append(Issue("error", src, f"{prop} is secret: reference it under 'secrets', never inline"))
            if prop not in env.secrets:
                issues.append(Issue("error", src, f"secret {prop} has no 'secrets' entry (fromEnv)"))
            continue
        if prop not in props:
            issues.append(Issue("error", src, f"site property {prop} has no desired value"))
            continue
        issues.extend(_check_value(prop, spec, props[prop], src))
    for prop in props:
        if prop not in specs:
            issues.append(Issue("error", src, f"unknown site property {prop} (not declared in application.json)"))
    for secret in env.secrets:
        if secret not in specs or not specs[secret].get("secret"):
            issues.append(Issue("error", src, f"'{secret}' is listed under secrets but is not a secret site property"))

    low, high = props.get("LowAvailabilityMaxPct"), props.get("HighAvailabilityMinPct")
    if isinstance(low, (int, float)) and isinstance(high, (int, float)) and not low < high:
        issues.append(
            Issue("error", src, f"LowAvailabilityMaxPct ({low}) must be below HighAvailabilityMinPct ({high})")
        )
    ttl, min_refresh = props.get("CacheTtlSeconds"), props.get("MinRefreshIntervalSeconds")
    if isinstance(ttl, int) and isinstance(min_refresh, int) and min_refresh > ttl:
        issues.append(
            Issue("error", src, f"MinRefreshIntervalSeconds ({min_refresh}) must not exceed CacheTtlSeconds ({ttl})")
        )
    stale = props.get("StaleAfterMinutes")
    if isinstance(stale, int) and isinstance(ttl, int) and stale * 60 <= ttl:
        issues.append(
            Issue(
                "error",
                src,
                f"StaleAfterMinutes ({stale}) must be longer than the cache TTL ({ttl}s), "
                "otherwise freshly cached data is reported as stale",
            )
        )

    checks_age = env.checks.get("maxDataAgeMinutes")
    if isinstance(stale, int) and isinstance(checks_age, int) and checks_age > stale:
        issues.append(
            Issue("warning", src, f"checks.maxDataAgeMinutes ({checks_age}) is looser than StaleAfterMinutes ({stale})")
        )

    if env.enabled and PLACEHOLDER_MARKER in json.dumps(env.raw):
        issues.append(Issue("error", src, f"enabled environment still contains {PLACEHOLDER_MARKER} placeholders"))
    if not env.enabled:
        issues.append(
            Issue("warning", src, "environment is disabled (fill in baseUrl/lifetimeEnvironmentName to enable)")
        )
    if env.is_lifetime and not env.lifetime_environment_name:
        issues.append(Issue("error", src, "lifetime environments need lifetimeEnvironmentName"))
    return issues


def pipeline_issues(pipeline: dict[str, Any], envs: dict[str, Environment]) -> list[Issue]:
    issues: list[Issue] = []
    for i, stage in enumerate(pipeline["stages"]):
        for end in ("from", "to"):
            env = envs.get(stage[end])
            if env is None:
                issues.append(
                    Issue("error", "pipeline.json", f"stage {i} {end}='{stage[end]}' is not a defined environment")
                )
            elif not env.is_lifetime:
                issues.append(
                    Issue("error", "pipeline.json", f"stage {i} uses '{env.name}', which is not a LifeTime environment")
                )
        if stage["from"] == stage["to"]:
            issues.append(Issue("error", "pipeline.json", f"stage {i} promotes an environment to itself"))
    targets = [s["to"] for s in pipeline["stages"]]
    prod = [s for s in pipeline["stages"] if s["to"] == "production"]
    if prod and not all(s["requiresApproval"] for s in prod):
        issues.append(Issue("error", "pipeline.json", "promotion to production must require approval"))
    if len(set(targets)) != len(targets):
        issues.append(Issue("error", "pipeline.json", "an environment is the target of more than one stage"))
    return issues


def validate_all(root: Path | None = None) -> list[Issue]:
    """Schema + semantic validation of the whole infra/ tree."""
    root = root or repo_root()
    schemas = root / "infra" / "schemas"
    issues: list[Issue] = []
    app = load_application(root)
    issues += _schema_issues(app, schemas / "application.schema.json", "application.json")
    envs: dict[str, Environment] = {}
    for name in environment_names(root):
        doc = _read_json(root / "infra" / "environments" / f"{name}.json")
        schema_errors = _schema_issues(doc, schemas / "environment.schema.json", f"environments/{name}.json")
        issues += schema_errors
        if doc.get("name") != name:
            issues.append(Issue("error", f"environments/{name}.json", f"'name' must equal the file name ({name})"))
        if not schema_errors:
            envs[name] = load_environment(name, root)
            issues += semantic_issues(app, envs[name])
    pipeline = load_pipeline(root)
    pipeline_schema = _schema_issues(pipeline, schemas / "pipeline.schema.json", "pipeline.json")
    issues += pipeline_schema
    if not pipeline_schema:
        issues += pipeline_issues(pipeline, envs)
    for module in app.get("modules", []):
        for key in ("oml", "manifest"):
            if not (root / module[key]).is_file():
                issues.append(
                    Issue("error", "application.json", f"module {module['name']}: {module[key]} does not exist")
                )
    return issues


def resolve_secret(env: Environment, prop: str) -> str | None:
    """Reads a secret from the process environment. Returns None when it is not set."""
    spec = env.secrets.get(prop)
    if not spec:
        return None
    value = os.environ.get(spec["fromEnv"], "")
    return value or None
