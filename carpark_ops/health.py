"""Reads the application's CarparkOps/Health endpoint and checks it against the desired state in infra/.

OutSystems omits default values (0, false, "") from exposed REST responses, so missing fields are filled with
their defaults before checking.
"""

from __future__ import annotations

from dataclasses import dataclass
from typing import Any

import requests

from .config import Environment, Issue

HEALTH_DEFAULTS: dict[str, Any] = {
    "status": "",
    "module": "",
    "snapshotAvailable": False,
    "snapshotAgeSeconds": 0,
    "dataAgeSeconds": 0,
    "sourceTimestamp": "",
    "carparkCount": 0,
    "rejectedCount": 0,
    "mergedRecordCount": 0,
    "inconsistentLotCount": 0,
    "hasLocationData": False,
    "carparkInfoCount": 0,
    "carparkInfoSyncAgeHours": 0,
    "highAvailabilityMinPct": 0.0,
    "lowAvailabilityMaxPct": 0.0,
    "staleAfterMinutes": 0,
    "readingOutdatedMinutes": 0,
    "cacheTtlSeconds": 0,
    "minRefreshIntervalSeconds": 0,
    "carparkInfoMaxAgeHours": 0,
    "hdbCarparkInfoResourceId": "",
    "dataGovApiKeyConfigured": False,
}


@dataclass
class DriftItem:
    site_property: str
    expected: Any
    actual: Any

    def __str__(self) -> str:
        return f"{self.site_property}: expected {self.expected!r}, deployed {self.actual!r}"


def fetch_health(
    env: Environment,
    health_path: str,
    *,
    refresh: bool = False,
    session: requests.Session | None = None,
    timeout: float = 90.0,
) -> dict[str, Any]:
    http = session or requests.Session()
    params = {"Refresh": "true"} if refresh else None
    resp = http.get(env.url(health_path), params=params, timeout=timeout, headers={"Accept": "application/json"})
    resp.raise_for_status()
    return normalize(resp.json())


def normalize(doc: dict[str, Any]) -> dict[str, Any]:
    return {**HEALTH_DEFAULTS, **doc}


def health_issues(health: dict[str, Any], env: Environment, module_name: str) -> list[Issue]:
    src = f"health[{env.name}]"
    issues: list[Issue] = []
    checks = env.checks
    if health["module"] != module_name:
        issues.append(Issue("error", src, f"endpoint reports module {health['module']!r}, expected {module_name!r}"))
    if not health["snapshotAvailable"]:
        issues.append(Issue("error", src, "no availability data has been retrieved yet (status no-data)"))
        return issues
    if health["status"] != "ok":
        issues.append(Issue("error", src, f"status is {health['status']!r}"))
    age = health["dataAgeSeconds"] if health["dataAgeSeconds"] >= 0 else health["snapshotAgeSeconds"]
    if age > checks["maxDataAgeMinutes"] * 60:
        issues.append(Issue("error", src, f"data is {age // 60} min old (limit {checks['maxDataAgeMinutes']} min)"))
    if health["carparkCount"] < checks["minCarparks"]:
        issues.append(
            Issue("error", src, f"only {health['carparkCount']} carparks (expected at least {checks['minCarparks']})")
        )
    if checks["requireLocationData"] and not health["hasLocationData"]:
        issues.append(Issue("error", src, "HDB address data is missing (CarparkInfo cache empty)"))
    if health["rejectedCount"] > max(10, health["carparkCount"] // 20):
        issues.append(Issue("warning", src, f"{health['rejectedCount']} upstream records were rejected as incomplete"))
    return issues


def drift(
    health: dict[str, Any], app: dict[str, Any], env: Environment, secret_present: dict[str, bool]
) -> list[DriftItem]:
    """Compares deployed site property values with infra/environments/<env>.json."""
    items: list[DriftItem] = []
    for prop, spec in app["siteProperties"].items():
        field_name = spec["healthField"]
        actual = health.get(field_name)
        if spec.get("secret"):
            required = env.secrets.get(prop, {}).get("required", False)
            if required and not actual:
                items.append(DriftItem(prop, "configured", "not configured"))
            elif secret_present.get(prop) is not None and secret_present[prop] != bool(actual):
                items.append(
                    DriftItem(
                        prop, "configured" if secret_present[prop] else "empty", "configured" if actual else "empty"
                    )
                )
            continue
        expected = env.site_properties.get(prop)
        if spec["type"] in ("decimal", "integer"):
            if actual is None or abs(float(actual) - float(expected)) > 1e-9:
                items.append(DriftItem(prop, expected, actual))
        elif actual != expected:
            items.append(DriftItem(prop, expected, actual))
    return items
