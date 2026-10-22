import responses

from carpark_ops import config, health


def personal():
    return config.load_environment("personal")


def test_omitted_defaults_are_filled(health_payload):
    report = health.normalize(health_payload)
    assert report["rejectedCount"] == 0 and report["dataGovApiKeyConfigured"] is False


def test_healthy_report_has_no_issues(health_payload):
    assert health.health_issues(health.normalize(health_payload), personal(), "CarparkAvailability") == []


def test_stale_and_missing_data_are_errors(health_payload):
    stale = health.normalize({**health_payload, "status": "stale", "dataAgeSeconds": 3600})
    msgs = [str(i) for i in health.health_issues(stale, personal(), "CarparkAvailability")]
    assert any("status is 'stale'" in m for m in msgs) and any("min old" in m for m in msgs)
    empty = health.normalize({"status": "no-data", "module": "CarparkAvailability"})
    assert any("no availability data" in str(i) for i in health.health_issues(empty, personal(), "CarparkAvailability"))


def test_low_carpark_count_and_missing_addresses(health_payload):
    report = health.normalize({**health_payload, "carparkCount": 10, "hasLocationData": False})
    msgs = " ".join(str(i) for i in health.health_issues(report, personal(), "CarparkAvailability"))
    assert "only 10 carparks" in msgs and "address data is missing" in msgs


def test_no_drift_when_deployed_matches_infra(health_payload):
    app = config.load_application()
    assert health.drift(health.normalize(health_payload), app, personal(), {}) == []


def test_drift_lists_each_mismatch(health_payload):
    app = config.load_application()
    report = health.normalize({**health_payload, "cacheTtlSeconds": 120, "hdbCarparkInfoResourceId": "d_x"})
    items = {d.site_property for d in health.drift(report, app, personal(), {})}
    assert items == {"CacheTtlSeconds", "HdbCarparkInfoResourceId"}


def test_required_secret_must_be_configured(health_payload):
    app = config.load_application()
    env = personal()
    env.secrets["DataGovApiKey"]["required"] = True
    items = health.drift(health.normalize(health_payload), app, env, {})
    assert [d.site_property for d in items] == ["DataGovApiKey"]


def test_secret_presence_is_compared_when_known(health_payload):
    app = config.load_application()
    items = health.drift(health.normalize(health_payload), app, personal(), {"DataGovApiKey": True})
    assert [str(d) for d in items] == ["DataGovApiKey: expected 'configured', deployed 'empty'"]


@responses.activate
def test_fetch_health_passes_refresh_flag(health_payload):
    env = personal()
    url = env.url("/CarparkAvailability/rest/CarparkOps/Health")
    responses.get(url, json=health_payload)
    report = health.fetch_health(env, "/CarparkAvailability/rest/CarparkOps/Health", refresh=True)
    assert report["carparkCount"] == 2007
    assert responses.calls[0].request.url.endswith("Health?Refresh=true")
