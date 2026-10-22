"""Static checks on the GitHub Actions workflows (they are part of the infrastructure code)."""

import re
from pathlib import Path

import pytest
import yaml

from carpark_ops import config
from tests.conftest import ROOT

WORKFLOWS = sorted((ROOT / ".github" / "workflows").glob("*.yml"))
UNTRUSTED = re.compile(r"\$\{\{\s*(inputs|github\.event)\.")


def load(path: Path):
    doc = yaml.safe_load(path.read_text(encoding="utf-8"))
    doc["on"] = doc.pop(True, doc.get("on"))  # YAML 1.1 reads the key `on` as True
    return doc


def run_scripts(doc):
    for job in doc["jobs"].values():
        for step in job.get("steps", []):
            if "run" in step:
                yield step["run"]


@pytest.mark.parametrize("path", WORKFLOWS, ids=lambda p: p.name)
def test_workflow_parses_and_is_least_privilege(path):
    doc = load(path)
    assert doc["permissions"] == {"contents": "read"}
    for job in doc["jobs"].values():
        assert job.get("timeout-minutes") or job.get("strategy") or "outputs" in job


@pytest.mark.parametrize("path", WORKFLOWS, ids=lambda p: p.name)
def test_no_untrusted_input_is_interpolated_into_shell(path):
    for script in run_scripts(load(path)):
        assert not UNTRUSTED.search(script), f"{path.name}: pass inputs via env, not ${{{{ }}}} in run:\n{script}"


@pytest.mark.parametrize("path", WORKFLOWS, ids=lambda p: p.name)
def test_actions_are_pinned_to_a_version(path):
    for job in load(path)["jobs"].values():
        for step in job.get("steps", []):
            if "uses" in step:
                assert re.search(r"@v\d+|@[0-9a-f]{40}$", step["uses"]), step["uses"]


def test_deploy_choices_match_the_pipeline():
    deploy = load(ROOT / ".github" / "workflows" / "deploy.yml")
    inputs = deploy["on"]["workflow_dispatch"]["inputs"]
    stages = config.load_pipeline()["stages"]
    assert set(inputs["from"]["options"]) == {s["from"] for s in stages}
    assert set(inputs["to"]["options"]) == {s["to"] for s in stages}
    assert deploy["jobs"]["deploy"]["environment"] == "${{ inputs.to }}"  # GitHub Environment approval gate
    assert deploy["concurrency"]["cancel-in-progress"] is False
