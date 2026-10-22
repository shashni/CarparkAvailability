import pytest

from carpark_ops import config, deploy
from carpark_ops.lifetime import DeploymentStatus

PIPELINE = {
    "stages": [
        {"from": "development", "to": "test", "requiresApproval": False, "createVersion": True},
        {"from": "test", "to": "production", "requiresApproval": True, "createVersion": False},
    ],
    "deployment": {"pollIntervalSeconds": 0, "timeoutMinutes": 1, "allowConflicts": False},
}
APP = {"lifetimeApplicationName": "CarparkAvailability"}


def env(name, kind="lifetime", enabled=True):
    return config.Environment(
        name=name,
        kind=kind,
        enabled=enabled,
        base_url=f"https://{name}.example.com",
        site_properties={},
        secrets={},
        checks={},
        lifetime_environment_name=name.capitalize(),
    )


class FakeLifeTime:
    """In-memory stand-in for LifeTimeClient that records every call."""

    def __init__(
        self, modified=True, base_version="ver-base", conflicts=None, statuses=("running", "finished_successful")
    ):
        self.calls = []
        self.modified, self.base_version = modified, base_version
        self.conflicts = conflicts or []
        self.statuses = list(statuses)

    def environment_key(self, name):
        return "key-" + name.lower()

    def application_key(self, name):
        return "app-key"

    def get_application(self, env_key, app_key):
        return {
            "AppStatusInEnvs": [
                {
                    "EnvironmentKey": env_key,
                    "IsModified": self.modified,
                    "BaseApplicationVersionKey": self.base_version,
                    "ConsistencyStatus": "OK",
                    "ModuleStatusInEnvs": [{"ModuleVersionKey": "mod-v1"}],
                }
            ]
        }

    def create_version(self, env_key, app_key, version, notes, modules):
        self.calls.append(("create_version", version, tuple(modules)))
        return "ver-new"

    def create_deployment(self, src, tgt, versions, notes):
        self.calls.append(("create_deployment", src, tgt, tuple(versions)))
        return "dep-1"

    def get_deployment(self, key):
        return {"ApplicationConflicts": self.conflicts, "ApplicationsToRedeploy": []}

    def delete_deployment(self, key):
        self.calls.append(("delete", key))

    def command(self, key, cmd):
        self.calls.append(("command", cmd))

    def wait(self, key, poll_seconds, timeout_seconds, on_poll=None):
        status = DeploymentStatus(self.statuses[-1], "", [])
        if on_poll:
            on_poll(status)
        return status


def test_dev_to_test_tags_a_version_when_modified():
    lt = FakeLifeTime(modified=True)
    plan = deploy.plan_promotion(lt, APP, PIPELINE, env("development"), env("test"), version="1.3.0", notes="n")
    assert plan.create_version and plan.module_version_keys == ["mod-v1"]
    result = deploy.apply_plan(lt, plan, PIPELINE, log=lambda *_: None)
    assert result.succeeded
    assert lt.calls[0] == ("create_version", "1.3.0", ("mod-v1",))
    assert lt.calls[1] == ("create_deployment", "key-development", "key-test", ("ver-new",))
    assert ("command", "start") in lt.calls


def test_version_required_when_tagging():
    with pytest.raises(deploy.DeploymentError, match="--version"):
        deploy.plan_promotion(FakeLifeTime(), APP, PIPELINE, env("development"), env("test"), version=None, notes="n")


def test_invalid_version_format():
    with pytest.raises(deploy.DeploymentError):
        deploy.plan_promotion(FakeLifeTime(), APP, PIPELINE, env("development"), env("test"), version="v1", notes="n")


def test_test_to_production_reuses_tagged_version_and_warns_about_untagged_changes():
    lt = FakeLifeTime(modified=True)
    plan = deploy.plan_promotion(lt, APP, PIPELINE, env("test"), env("production"), version=None, notes="n")
    assert not plan.create_version and plan.base_version_key == "ver-base"
    assert any("NOT promoted" in w for w in plan.warnings)
    deploy.apply_plan(lt, plan, PIPELINE, log=lambda *_: None)
    assert lt.calls[0] == ("create_deployment", "key-test", "key-production", ("ver-base",))


def test_stage_must_exist_in_pipeline():
    with pytest.raises(deploy.DeploymentError, match="not a pipeline stage"):
        deploy.plan_promotion(
            FakeLifeTime(), APP, PIPELINE, env("development"), env("production"), version="1.0", notes=""
        )


def test_disabled_or_personal_environments_are_refused():
    with pytest.raises(deploy.DeploymentError, match="disabled"):
        deploy.plan_promotion(
            FakeLifeTime(), APP, PIPELINE, env("development", enabled=False), env("test"), version="1.0", notes=""
        )
    personal_pipeline = {
        **PIPELINE,
        "stages": [{"from": "personal", "to": "test", "requiresApproval": False, "createVersion": True}],
    }
    with pytest.raises(deploy.DeploymentError, match="not managed by LifeTime"):
        deploy.plan_promotion(
            FakeLifeTime(),
            APP,
            personal_pipeline,
            env("personal", kind="personal"),
            env("test"),
            version="1.0",
            notes="",
        )


def test_conflicts_discard_the_deployment():
    lt = FakeLifeTime(conflicts=[{"Application": "Other"}])
    plan = deploy.plan_promotion(lt, APP, PIPELINE, env("test"), env("production"), version=None, notes="n")
    with pytest.raises(deploy.DeploymentError, match="conflict"):
        deploy.apply_plan(lt, plan, PIPELINE, log=lambda *_: None)
    assert ("delete", "dep-1") in lt.calls and ("command", "start") not in lt.calls


@pytest.mark.parametrize("status", ["finished_with_errors", "aborted", "needs_user_intervention"])
def test_failed_terminal_status_is_reported(status):
    lt = FakeLifeTime(statuses=(status,))
    plan = deploy.plan_promotion(lt, APP, PIPELINE, env("test"), env("production"), version=None, notes="n")
    result = deploy.apply_plan(lt, plan, PIPELINE, log=lambda *_: None)
    assert not result.succeeded and result.status == status


def test_plan_description_is_human_readable():
    plan = deploy.plan_promotion(
        FakeLifeTime(), APP, PIPELINE, env("development"), env("test"), version="2.0", notes="n"
    )
    text = plan.describe()
    assert "development" in text and "create 2.0" in text
