import pytest

from carpark_ops import rules
from carpark_ops.rules import FULL, HIGH, LOW, MODERATE, UNKNOWN


@pytest.mark.parametrize(
    ("available", "total", "expected"),
    [
        (0, 46, FULL),
        (1, 100, LOW),  # 1% < 20%
        (19, 100, LOW),
        (20, 100, MODERATE),  # exactly the low threshold is Moderate
        (50, 100, MODERATE),  # exactly the high threshold is Moderate ("more than" 50% is High)
        (51, 100, HIGH),
        (100, 100, HIGH),
        (0, 0, UNKNOWN),  # no lots reported
        (12, 10, UNKNOWN),  # more available than total
        (-1, 10, UNKNOWN),
    ],
)
def test_classify_bands(available, total, expected):
    assert rules.classify(available, total, high_min_pct=50, low_max_pct=20) == expected


def test_classify_uses_configured_thresholds():
    assert rules.classify(30, 100, high_min_pct=25, low_max_pct=10) == HIGH
    assert rules.classify(30, 100, high_min_pct=80, low_max_pct=40) == LOW


def test_split_records_are_merged_into_one_carpark(availability_payload):
    batch = rules.map_batch(availability_payload)
    numbers = [cp.number for cp in batch.carparks]
    assert len(numbers) == len(set(numbers)) == 24  # 29 records, 24 distinct carparks
    assert batch.merged == 5  # C17, SK48, BM28, C4M, JM33
    sk48 = next(cp for cp in batch.carparks if cp.number == "SK48")
    assert sorted(lot.lot_type for lot in sk48.lots) == ["C", "H", "Y"]
    assert sk48.primary.lot_type == "C"
    assert (sk48.primary.available, sk48.primary.total) == (448, 471)


def test_primary_lot_skips_inconsistent_car_lots(availability_payload):
    batch = rules.map_batch(availability_payload)
    jm33 = next(cp for cp in batch.carparks if cp.number == "JM33")
    assert jm33.primary.lot_type == "C" and not jm33.primary.anomalous
    pl40 = next(cp for cp in batch.carparks if cp.number == "PL40")  # only an inconsistent 0/0 car reading
    assert pl40.primary.anomalous and pl40.primary.status == UNKNOWN


def test_inconsistent_readings_are_counted_not_dropped(availability_payload):
    batch = rules.map_batch(availability_payload)
    assert batch.inconsistent_lots == 2  # PL40 C 0/0 and JM33 S 0/0
    assert batch.rejected == 0


def test_edge_cases(edge_case_payload):
    batch = rules.map_batch(edge_case_payload)
    numbers = {cp.number for cp in batch.carparks}
    assert numbers == {"OVER", "MIX1", "EDGE", "HALF"}  # blank number, empty lots and non-numeric-only rejected
    assert batch.rejected == 3
    assert batch.inconsistent_lots == 3  # BADNUM text, OVER 12/10, MIX1 C 0/0
    mix = next(cp for cp in batch.carparks if cp.number == "MIX1")
    assert mix.primary.lot_type == "Y"  # car lots exist but are inconsistent
    edge = next(cp for cp in batch.carparks if cp.number == "EDGE")
    assert edge.lots[0].updated_at is None  # unparseable time kept as unknown, not invented


def test_summary_excludes_inconsistent_lots_from_totals(edge_case_payload):
    summary = rules.summarize(rules.map_batch(edge_case_payload))
    assert summary.carparks == 4
    assert summary.total_lots == 20 + 100 + 100  # MIX1 Y, EDGE, HALF; OVER is inconsistent
    assert summary.available_lots == 4 + 20 + 50
    assert summary.average_pct == round((20.0 + 20.0 + 50.0) / 3, 1)


def test_summary_for_one_lot_type(availability_payload):
    batch = rules.map_batch(availability_payload)
    motorcycles = rules.summarize(batch, lot_type="Y")
    assert motorcycles.carparks == sum(1 for cp in batch.carparks for lot in cp.lots if lot.lot_type == "Y")
    assert motorcycles.carparks >= 4


def test_full_or_low_counts_full_carparks(availability_payload):
    summary = rules.summarize(rules.map_batch(availability_payload))
    assert summary.full_or_low >= 2  # BM3 and T18 report 0 car lots


@pytest.mark.parametrize(
    "payload",
    [
        {},
        {"items": []},
        {"items": [{"timestamp": "x", "carpark_data": []}]},
        {"items": [{"timestamp": "bad", "carpark_data": [{"carpark_number": "A"}]}]},
    ],
)
def test_unusable_batches_raise(payload):
    with pytest.raises(ValueError):
        rules.map_batch(payload)
