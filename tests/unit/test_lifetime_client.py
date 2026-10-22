import json

import pytest
import responses

from carpark_ops.lifetime import LifeTimeClient, LifeTimeError, module_version_keys

LT = "https://lt.example.com"
API = LT + "/lifetimeapi/rest/v2"
ENVS = [{"Key": "env-dev", "Name": "Development"}, {"Key": "env-prd", "Name": "Production"}]


def client(**kw):
    return LifeTimeClient(LT + "/lifetime", "tok-123", sleep=lambda s: None, **kw)


def test_rejects_insecure_url_and_missing_token():
    with pytest.raises(ValueError):
        LifeTimeClient("http://lt.example.com", "t")
    with pytest.raises(ValueError):
        LifeTimeClient(LT, "")


@responses.activate
def test_sends_bearer_token_and_resolves_environment_by_name():
    responses.get(API + "/environments/", json=ENVS)
    assert client().environment_key("production") == "env-prd"
    assert responses.calls[0].request.headers["Authorization"] == "Bearer tok-123"


@responses.activate
def test_unknown_environment_lists_known_names():
    responses.get(API + "/environments/", json=ENVS)
    with pytest.raises(LifeTimeError, match="Development"):
        client().environment_key("Staging")


@responses.activate
def test_create_version_body():
    responses.post(API + "/environments/env-dev/applications/app-1/versions/", json={"ApplicationVersionKey": "ver-9"})
    key = client().create_version("env-dev", "app-1", "1.2.0", "notes", ["mod-a"])
    assert key == "ver-9"
    body = json.loads(responses.calls[0].request.body)
    assert body == {"ChangeLog": "notes", "Version": "1.2.0", "MobileVersions": [], "ModuleVersionKeys": ["mod-a"]}


@responses.activate
def test_create_deployment_body_and_string_key():
    responses.post(API + "/deployments/", json="dep-1")
    assert client().create_deployment("env-dev", "env-prd", ["ver-9"], "go") == "dep-1"
    body = json.loads(responses.calls[0].request.body)
    assert body["SourceEnvironmentKey"] == "env-dev" and body["TargetEnvironmentKey"] == "env-prd"
    assert body["ApplicationOperations"] == [{"ApplicationVersionKey": "ver-9", "DeploymentZoneKey": ""}]


@responses.activate
def test_start_command_accepts_202_without_body():
    responses.post(API + "/deployments/dep-1/start/", status=202)
    client().command("dep-1", "start")


def test_unknown_command_is_refused():
    with pytest.raises(ValueError):
        client().command("dep-1", "rollback")


@responses.activate
def test_transient_errors_are_retried_then_succeed():
    responses.get(API + "/environments/", status=503)
    responses.get(API + "/environments/", status=502)
    responses.get(API + "/environments/", json=ENVS)
    assert client(retries=3).list_environments() == ENVS
    assert len(responses.calls) == 3


@responses.activate
def test_client_errors_are_not_retried():
    responses.get(API + "/environments/", status=401, body="invalid token")
    with pytest.raises(LifeTimeError) as exc:
        client(retries=3).list_environments()
    assert exc.value.status == 401 and len(responses.calls) == 1


@responses.activate
def test_retries_exhausted():
    for _ in range(3):
        responses.get(API + "/environments/", status=503)
    with pytest.raises(LifeTimeError, match="503"):
        client(retries=2).list_environments()


@responses.activate
def test_wait_polls_until_terminal_status():
    for status in ("running", "running", "finished_successful"):
        responses.get(
            API + "/deployments/dep-1/status/", json={"DeploymentStatus": status, "Info": "", "DeploymentLog": []}
        )
    seen = []
    final = client().wait("dep-1", poll_seconds=0, timeout_seconds=60, on_poll=lambda s: seen.append(s.status))
    assert final.succeeded and seen == ["running", "running", "finished_successful"]


@responses.activate
def test_wait_times_out():
    responses.get(API + "/deployments/dep-1/status/", json={"DeploymentStatus": "running"})
    with pytest.raises(LifeTimeError, match="still 'running'"):
        client().wait("dep-1", poll_seconds=0, timeout_seconds=0)


def test_module_version_keys_from_environment_status():
    app = {
        "AppStatusInEnvs": [
            {
                "EnvironmentKey": "env-dev",
                "ModuleStatusInEnvs": [{"ModuleVersionKey": "m1"}, {"ModuleVersionKey": "m2"}],
            },
            {"EnvironmentKey": "env-prd", "ModuleStatusInEnvs": [{"ModuleVersionKey": "old"}]},
        ]
    }
    assert module_version_keys(app, "env-dev") == ["m1", "m2"]


def test_module_version_keys_fallback_to_modules_list():
    app = {
        "AppStatusInEnvs": [{"EnvironmentKey": "env-dev"}],
        "Modules": [{"ModuleStatusInEnvs": [{"EnvironmentKey": "env-dev", "ModuleVersionKey": "m9"}]}],
    }
    assert module_version_keys(app, "env-dev") == ["m9"]
