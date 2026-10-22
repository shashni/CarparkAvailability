"""Live checks against a deployed environment (default: personal). Run: CARPARK_ENV=personal pytest -m smoke"""

import os

import pytest
import requests

from carpark_ops import config, datagov, health, rules

pytestmark = pytest.mark.smoke
ENV_NAME = os.environ.get("CARPARK_ENV", "personal")


@pytest.fixture(scope="module")
def app():
    return config.load_application()


@pytest.fixture(scope="module")
def env():
    environment = config.load_environment(ENV_NAME)
    if not environment.enabled:
        pytest.skip(f"environment {ENV_NAME} is disabled")
    return environment


@pytest.fixture(scope="module")
def report(app, env):
    return health.fetch_health(env, app["paths"]["health"], refresh=True)


def test_dashboard_shell_is_served(app, env):
    resp = requests.get(env.url(app["paths"]["dashboard"]), timeout=60)
    assert resp.status_code == 200
    assert f"basePath: '/{app['modules'][0]['name']}/'" in resp.text


def test_health_is_ok(app, env, report):
    assert health.health_issues(report, env, app["modules"][0]["name"]) == []


def test_health_matches_contract(report):
    from jsonschema import Draft202012Validator

    assert list(Draft202012Validator(datagov._schema("health.schema.json")).iter_errors(report)) == []


def test_no_configuration_drift(app, env, report):
    assert [str(d) for d in health.drift(report, app, env, {})] == []


def test_carpark_count_matches_reference_rules(report, env):
    expected = rules.map_batch(datagov.fetch_availability(config.resolve_secret(env, "DataGovApiKey")))
    assert abs(report["carparkCount"] - len(expected.carparks)) <= max(5, len(expected.carparks) // 100)


def test_health_endpoint_exposes_no_secret_values(report):
    text = str(report)
    key = config.resolve_secret(config.load_environment(ENV_NAME), "DataGovApiKey")
    if key:
        assert key not in text
    assert "apiKey" not in report and "DataGovApiKey" not in report
