"""The recorded upstream samples and the health sample must satisfy the contracts the module relies on."""

from jsonschema import Draft202012Validator

from carpark_ops import datagov, health


def test_availability_sample_matches_contract(availability_payload):
    assert datagov.contract_errors(availability_payload, "availability.schema.json") == []


def test_hdb_sample_matches_contract(hdb_payload):
    assert datagov.contract_errors(hdb_payload, "datastore.schema.json") == []


def test_contract_detects_numeric_lot_counts(availability_payload):
    availability_payload["items"][0]["carpark_data"][0]["carpark_info"][0]["total_lots"] = 105  # number, not text
    assert datagov.contract_errors(availability_payload, "availability.schema.json")


def test_contract_detects_missing_fields(availability_payload):
    del availability_payload["items"][0]["carpark_data"][0]["update_datetime"]
    assert any(
        "update_datetime" in e for e in datagov.contract_errors(availability_payload, "availability.schema.json")
    )


def test_health_sample_matches_contract(health_payload):
    schema = datagov._schema("health.schema.json")
    assert list(Draft202012Validator(schema).iter_errors(health.normalize(health_payload))) == []


def test_health_contract_rejects_unknown_status(health_payload):
    schema = datagov._schema("health.schema.json")
    bad = health.normalize({**health_payload, "status": "great"})
    assert list(Draft202012Validator(schema).iter_errors(bad))
