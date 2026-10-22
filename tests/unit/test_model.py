import json

from carpark_ops import cli, config, model
from tests.conftest import ROOT


def test_committed_module_package_is_intact():
    app = config.load_application()
    assert cli.model_issues(ROOT, app) == []


def test_oml_signature_and_checksum(repo_copy):
    app = config.load_application(repo_copy)
    module = app["modules"][0]
    oml = repo_copy / module["oml"]
    manifest = model.load_manifest(repo_copy, module)
    assert model.verify_oml(oml, manifest) == []
    data = bytearray(oml.read_bytes())
    data[-1] ^= 0xFF  # flip one byte
    oml.write_bytes(bytes(data))
    assert any("sha256" in p for p in model.verify_oml(oml, manifest))


def test_non_oml_file_is_detected(repo_copy):
    app = config.load_application(repo_copy)
    module = app["modules"][0]
    (repo_copy / module["oml"]).write_bytes(b"PK\x03\x04 not an oml" * 10)
    problems = model.verify_oml(repo_copy / module["oml"], model.load_manifest(repo_copy, module))
    assert any("signature" in p for p in problems)


def test_snapshot_edits_require_manifest_update(repo_copy):
    target = repo_copy / "src/model/action-AvailabilityStatus_Classify.cs"
    target.write_text(target.read_text(encoding="utf-8") + "// edited\n", encoding="utf-8")
    issues = cli.model_issues(repo_copy, config.load_application(repo_copy))
    assert any("changed without" in str(i) for i in issues)


def test_site_properties_in_module_match_infra():
    in_model = model.site_properties_in_model(ROOT / "src/model")
    declared = config.load_application()["siteProperties"]
    assert set(in_model) == set(declared)


def test_site_property_missing_from_infra_is_reported(repo_copy):
    app_path = repo_copy / "infra/application.json"
    doc = json.loads(app_path.read_text(encoding="utf-8"))
    del doc["siteProperties"]["CacheTtlSeconds"]
    app_path.write_text(json.dumps(doc), encoding="utf-8")
    issues = cli.model_issues(repo_copy, config.load_application(repo_copy))
    assert any("module defines site property CacheTtlSeconds" in str(i) for i in issues)


def test_inventory_lists_core_elements():
    inv = model.inventory(ROOT / "src/model")
    assert "Carpark_GetAvailability" in inv["serverActions"]
    assert {"DataGovSG_Transport", "DataGovSG_Datastore", "CarparkOps"} <= set(inv["integrations"])
    assert "Dashboard" in inv["screens"]


def test_normalize_snapshot_strips_fence_and_bom():
    raw = "﻿```csharp\n(OutSystems.Model.IESpace eSpace) => {\n}\n```\n"
    assert model.normalize_snapshot(raw) == "(OutSystems.Model.IESpace eSpace) => {\n}\n"


def test_normalize_export_renames(tmp_path):
    (tmp_path / "a.txt").write_text("```csharp\nx\n```", encoding="utf-8")
    (tmp_path / "b.txt").write_text('{"data": 1}', encoding="utf-8")
    written = sorted(p.name for p in model.normalize_export(tmp_path))
    assert written == ["a.cs", "b.json"] and not list(tmp_path.glob("*.txt"))
