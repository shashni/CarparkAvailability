import json
import shutil
from pathlib import Path

import pytest

ROOT = Path(__file__).resolve().parents[1]
FIXTURES = Path(__file__).parent / "fixtures"


def load_fixture(name: str):
    return json.loads((FIXTURES / name).read_text(encoding="utf-8"))


@pytest.fixture
def availability_payload():
    return load_fixture("availability_sample.json")


@pytest.fixture
def edge_case_payload():
    return load_fixture("availability_edge_cases.json")


@pytest.fixture
def hdb_payload():
    return load_fixture("hdb_info_sample.json")


@pytest.fixture
def health_payload():
    return load_fixture("health_sample.json")


@pytest.fixture
def repo_copy(tmp_path, monkeypatch):
    """A writable copy of the repository's infra/ and src/ that tests can break on purpose."""
    for folder in ("infra", "src"):
        shutil.copytree(ROOT / folder, tmp_path / folder)
    monkeypatch.setenv("CARPARK_REPO_ROOT", str(tmp_path))
    return tmp_path


def edit_json(path: Path, mutate):
    doc = json.loads(path.read_text(encoding="utf-8"))
    mutate(doc)
    path.write_text(json.dumps(doc, indent=2), encoding="utf-8")
