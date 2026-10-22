"""Live contract tests: does data.gov.sg still return what the module's REST structures expect?

Run: pytest -m contract   (scheduled daily in .github/workflows/contract.yml)
"""

import os

import pytest

from carpark_ops import datagov, rules

pytestmark = pytest.mark.contract
API_KEY = os.environ.get("CARPARK_DATAGOV_API_KEY") or None
HDB_RESOURCE = "d_23f946fa557947f93a8043bbef41dd09"


@pytest.fixture(scope="module")
def availability():
    return datagov.fetch_availability(API_KEY)


def test_availability_matches_contract(availability):
    assert datagov.contract_errors(availability, "availability.schema.json") == []


def test_availability_is_current(availability):
    from datetime import UTC, datetime, timedelta

    stamp = datetime.fromisoformat(availability["items"][0]["timestamp"])
    assert datetime.now(UTC) - stamp < timedelta(minutes=30), f"upstream timestamp {stamp} is stale"


def test_availability_maps_to_a_plausible_batch(availability):
    batch = rules.map_batch(availability)
    assert len(batch.carparks) > 1500
    assert batch.rejected < 50
    assert {lot.lot_type for cp in batch.carparks for lot in cp.lots} <= {"C", "Y", "H", "S", "L"}


def test_hdb_info_matches_contract():
    info = datagov.fetch_hdb_info(HDB_RESOURCE, api_key=API_KEY)
    assert datagov.contract_errors(info, "datastore.schema.json") == []
    assert len(info["result"]["records"]) > 2000
