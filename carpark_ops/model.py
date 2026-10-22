"""Module source artifacts: the binary .oml, its manifest, and the reviewable text snapshots in src/model/.

The .oml is what gets published. The text snapshots are Model API renderings of the same module, exported with
the Service Studio MCP read tools (see docs/DEPLOYMENT.md), so changes can be reviewed in ordinary diffs.
"""

from __future__ import annotations

import hashlib
import json
import re
from datetime import UTC, datetime
from pathlib import Path
from typing import Any

OML_MAGIC = b"OMLP"
_FENCE = re.compile(r"^\s*```[a-z]*\s*\n|\n```\s*$")


def sha256_of(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as fh:
        for chunk in iter(lambda: fh.read(1 << 20), b""):
            digest.update(chunk)
    return digest.hexdigest()


def verify_oml(oml: Path, manifest: dict[str, Any]) -> list[str]:
    """Returns problems with the committed module package (empty list when it is intact)."""
    problems: list[str] = []
    if not oml.is_file():
        return [f"{oml} is missing"]
    with oml.open("rb") as fh:
        head = fh.read(4)
    if head != OML_MAGIC:
        problems.append(f"{oml.name} does not start with the OML signature (found {head!r})")
    size = oml.stat().st_size
    if size < 100_000:
        problems.append(f"{oml.name} is suspiciously small ({size} bytes)")
    expected = manifest.get("oml", {}).get("sha256")
    actual = sha256_of(oml)
    if expected != actual:
        problems.append(
            f"{oml.name} sha256 {actual} does not match the manifest ({expected}); "
            "run `carpark-ops update-manifest` after exporting a new module version"
        )
    return problems


def normalize_snapshot(text: str) -> str:
    """Strips the BOM and markdown code fence the MCP read tools wrap around Model API code."""
    text = text.lstrip("﻿").replace("\r\n", "\n")
    text = _FENCE.sub("", text.strip()).strip()
    return text + "\n"


def normalize_export(model_dir: Path) -> list[Path]:
    """Turns raw *.txt exports into *.cs (Model API code) or *.json (query results)."""
    written: list[Path] = []
    for raw in sorted(model_dir.glob("*.txt")):
        body = normalize_snapshot(raw.read_text(encoding="utf-8-sig"))
        suffix = ".json" if body.lstrip().startswith(("{", "[")) else ".cs"
        target = raw.with_suffix(suffix)
        target.write_text(body, encoding="utf-8", newline="\n")
        raw.unlink()
        written.append(target)
    return written


def site_properties_in_model(model_dir: Path) -> dict[str, str]:
    """Site property name -> Model API data type, read from src/model/site-properties.cs."""
    text = (model_dir / "site-properties.cs").read_text(encoding="utf-8")
    found: dict[str, str] = {}
    for var, name in re.findall(r'var (\w+) = eSpace\.CreateSiteProperty\(\w+, "(\w+)"\)', text):
        match = re.search(rf"{var}\.DataType = eSpace\.(\w+)Type;", text)
        found[name] = match.group(1) if match else "Unknown"
    return found


def inventory(model_dir: Path) -> dict[str, list[str]]:
    """Named elements per kind, parsed from the text snapshots (used by the manifest and tests)."""

    def grab(file: str, pattern: str) -> list[str]:
        path = model_dir / file
        return sorted(set(re.findall(pattern, path.read_text(encoding="utf-8")))) if path.is_file() else []

    refs_doc = json.loads((model_dir / "inventory.json").read_text(encoding="utf-8"))
    return {
        "references": sorted(r["Name"] for r in refs_doc["data"]["Root"]["References"]),
        "entities": grab("data-model.cs", r'Create(?:Server|Static)Entity\("(\w+)"'),
        "structures": grab("structures.cs", r'CreateStructure\("(\w+)"'),
        "integrations": grab("integrations.cs", r'CreateIntegration<[\w.]+>\("(\w+)"'),
        "siteProperties": sorted(site_properties_in_model(model_dir)),
        "serverActions": sorted(p.stem.removeprefix("action-") for p in model_dir.glob("action-*.cs")),
        "screens": sorted(p.stem.removeprefix("screen-") for p in model_dir.glob("screen-*.cs")),
    }


def build_manifest(root: Path, module: dict[str, Any], exported_from: str) -> dict[str, Any]:
    oml = root / module["oml"]
    model_dir = root / "src" / "model"
    return {
        "module": module["name"],
        "moduleKey": module.get("key"),
        "type": module["type"],
        "exportedAt": datetime.now(UTC).replace(microsecond=0).isoformat(),
        "exportedFrom": exported_from,
        "oml": {"path": module["oml"], "sha256": sha256_of(oml), "bytes": oml.stat().st_size},
        "snapshots": {p.name: sha256_of(p) for p in sorted(model_dir.glob("*.*"))},
        "inventory": inventory(model_dir),
    }


def load_manifest(root: Path, module: dict[str, Any]) -> dict[str, Any]:
    return json.loads((root / module["manifest"]).read_text(encoding="utf-8"))
