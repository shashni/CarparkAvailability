from carpark_ops import config
from tests.conftest import edit_json


def errors(issues):
    return [str(i) for i in issues if i.severity == "error"]


def test_repository_configuration_is_valid():
    assert errors(config.validate_all()) == []


def test_disabled_environments_are_warnings_not_errors():
    warnings = [i for i in config.validate_all() if i.severity == "warning"]
    assert {i.source for i in warnings} >= {"environments/production.json"}


def test_every_environment_sets_every_non_secret_property():
    app = config.load_application()
    for name in config.environment_names():
        env = config.load_environment(name)
        for prop, spec in app["siteProperties"].items():
            if spec.get("secret"):
                assert prop in env.secrets and prop not in env.site_properties
            else:
                assert prop in env.site_properties, f"{name} is missing {prop}"


def test_threshold_order_is_enforced(repo_copy):
    edit_json(
        repo_copy / "infra/environments/personal.json", lambda d: d["siteProperties"].update(LowAvailabilityMaxPct=60)
    )
    assert any("must be below HighAvailabilityMinPct" in e for e in errors(config.validate_all()))


def test_stale_window_must_exceed_cache_ttl(repo_copy):
    edit_json(
        repo_copy / "infra/environments/personal.json",
        lambda d: d["siteProperties"].update(StaleAfterMinutes=1, CacheTtlSeconds=120, MinRefreshIntervalSeconds=15),
    )
    assert any("longer than the cache TTL" in e for e in errors(config.validate_all()))


def test_inline_secret_is_rejected(repo_copy):
    edit_json(
        repo_copy / "infra/environments/personal.json", lambda d: d["siteProperties"].update(DataGovApiKey="abc123")
    )
    assert any("is secret" in e for e in errors(config.validate_all()))


def test_unknown_property_is_rejected(repo_copy):
    edit_json(repo_copy / "infra/environments/personal.json", lambda d: d["siteProperties"].update(Typo=1))
    assert any("unknown site property Typo" in e for e in errors(config.validate_all()))


def test_out_of_range_value_is_rejected(repo_copy):
    edit_json(
        repo_copy / "infra/environments/personal.json", lambda d: d["siteProperties"].update(HighAvailabilityMinPct=150)
    )
    assert any("above the maximum" in e for e in errors(config.validate_all()))


def test_resource_id_pattern(repo_copy):
    edit_json(
        repo_copy / "infra/environments/personal.json",
        lambda d: d["siteProperties"].update(HdbCarparkInfoResourceId="not-an-id"),
    )
    assert any("does not match" in e for e in errors(config.validate_all()))


def test_enabled_environment_with_placeholders_is_rejected(repo_copy):
    edit_json(repo_copy / "infra/environments/production.json", lambda d: d.update(enabled=True))
    assert any("REPLACE_ME" in e for e in errors(config.validate_all()))


def test_schema_violation_is_reported(repo_copy):
    edit_json(repo_copy / "infra/environments/personal.json", lambda d: d.update(baseUrl="http://insecure"))
    assert any("baseUrl" in e for e in errors(config.validate_all()))


def test_production_promotion_requires_approval(repo_copy):
    edit_json(repo_copy / "infra/pipeline.json", lambda d: d["stages"][1].update(requiresApproval=False))
    assert any("must require approval" in e for e in errors(config.validate_all()))


def test_pipeline_cannot_target_personal(repo_copy):
    edit_json(repo_copy / "infra/pipeline.json", lambda d: d["stages"][0].update(to="personal"))
    assert any("not a LifeTime environment" in e for e in errors(config.validate_all()))


def test_local_override_file_is_merged(repo_copy):
    (repo_copy / "infra/environments/personal.local.json").write_text('{"baseUrl": "https://other.example.com"}')
    assert config.load_environment("personal").base_url == "https://other.example.com"


def test_secret_resolution_reads_process_environment(monkeypatch):
    env = config.load_environment("personal")
    monkeypatch.setenv("CARPARK_DATAGOV_API_KEY", "")
    assert config.resolve_secret(env, "DataGovApiKey") is None
    monkeypatch.setenv("CARPARK_DATAGOV_API_KEY", "k-123")
    assert config.resolve_secret(env, "DataGovApiKey") == "k-123"
