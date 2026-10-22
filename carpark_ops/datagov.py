"""data.gov.sg client and response contracts (the shapes the module's REST structures depend on)."""

from __future__ import annotations

import json
from importlib import resources
from typing import Any

import requests
from jsonschema import Draft202012Validator

AVAILABILITY_URL = "https://api.data.gov.sg/v1/transport/carpark-availability"
DATASTORE_URL = "https://data.gov.sg/api/action/datastore_search"
USER_AGENT = "carpark-ops/1.0 (+https://github.com/shashni/CarparkAvailability)"


def _schema(name: str) -> dict[str, Any]:
    return json.loads(resources.files("carpark_ops").joinpath("schemas", name).read_text(encoding="utf-8"))


def contract_errors(doc: Any, schema_name: str, limit: int = 20) -> list[str]:
    validator = Draft202012Validator(_schema(schema_name))
    errors = []
    for err in validator.iter_errors(doc):
        errors.append(f"{'/'.join(str(p) for p in err.absolute_path) or '<root>'}: {err.message}")
        if len(errors) >= limit:
            break
    return errors


def _headers(api_key: str | None) -> dict[str, str]:
    headers = {"Accept": "application/json", "User-Agent": USER_AGENT}
    if api_key:
        headers["x-api-key"] = api_key
    return headers


def fetch_availability(
    api_key: str | None = None, *, session: requests.Session | None = None, timeout: float = 30.0
) -> dict[str, Any]:
    http = session or requests.Session()
    resp = http.get(AVAILABILITY_URL, headers=_headers(api_key), timeout=timeout)
    resp.raise_for_status()
    return resp.json()


def fetch_hdb_info(
    resource_id: str,
    limit: int = 5000,
    api_key: str | None = None,
    *,
    session: requests.Session | None = None,
    timeout: float = 60.0,
) -> dict[str, Any]:
    http = session or requests.Session()
    resp = http.get(
        DATASTORE_URL, params={"resource_id": resource_id, "limit": limit}, headers=_headers(api_key), timeout=timeout
    )
    resp.raise_for_status()
    return resp.json()
