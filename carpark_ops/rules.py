"""Executable specification of the dashboard's data rules.

This mirrors the OutSystems implementation (Carpark_FetchAndMap, AvailabilityStatus_Classify and
Dashboard.ApplyFilters). Unit tests pin the behaviour; the smoke suite compares the deployed app's counters with
this implementation run on the same data.gov.sg payload, which catches regressions in the low-code logic.
"""

from __future__ import annotations

from collections import OrderedDict
from dataclasses import dataclass, field
from datetime import datetime
from typing import Any

HIGH, MODERATE, LOW, FULL, UNKNOWN = "High", "Moderate", "Low", "Full", "Unknown"


def classify(available: int, total: int, high_min_pct: float, low_max_pct: float) -> str:
    """AvailabilityStatus_Classify: thresholds come from site properties."""
    if total <= 0 or available < 0 or available > total:
        return UNKNOWN
    if available == 0:
        return FULL
    pct = available * 100.0 / total
    if pct < low_max_pct:
        return LOW
    if pct > high_min_pct:
        return HIGH
    return MODERATE


@dataclass
class Lot:
    lot_type: str
    total: int
    available: int
    updated_at: datetime | None
    status: str

    @property
    def anomalous(self) -> bool:
        return self.total <= 0 or self.available < 0 or self.available > self.total

    @property
    def pct(self) -> float:
        return 0.0 if self.anomalous else round(self.available * 100.0 / self.total, 1)


@dataclass
class Carpark:
    number: str
    lots: list[Lot] = field(default_factory=list)

    @property
    def primary(self) -> Lot:
        """Valid car lots when present, otherwise the first lot type received."""
        return next((lot for lot in self.lots if lot.lot_type == "C" and not lot.anomalous), self.lots[0])


@dataclass
class MappedBatch:
    source_timestamp: datetime
    carparks: list[Carpark]
    rejected: int
    merged: int
    inconsistent_lots: int


def _parse_time(text: str) -> datetime | None:
    try:
        return datetime.fromisoformat(text[:19])
    except (TypeError, ValueError):
        return None


def _int(text: Any) -> int | None:
    try:
        return int(str(text).strip())
    except (TypeError, ValueError):
        return None


def map_batch(payload: dict[str, Any], high_min_pct: float = 50, low_max_pct: float = 20) -> MappedBatch:
    """Carpark_FetchAndMap: validate, merge split records, classify per lot type."""
    items = payload.get("items") or []
    if not items or not items[0].get("carpark_data"):
        raise ValueError("no usable items")
    source = _parse_time(items[0].get("timestamp", ""))
    if source is None:
        raise ValueError("batch has no valid timestamp")

    carparks: OrderedDict[str, Carpark] = OrderedDict()
    rejected = merged = inconsistent = 0
    for record in items[0]["carpark_data"]:
        number = str(record.get("carpark_number") or "").strip().upper()
        info = record.get("carpark_info") or []
        if not number or not info:
            rejected += 1
            continue
        updated = _parse_time(record.get("update_datetime", ""))
        lots: list[Lot] = []
        for raw in info:
            total, available = _int(raw.get("total_lots")), _int(raw.get("lots_available"))
            if total is None or available is None:
                inconsistent += 1
                continue
            lot = Lot(
                str(raw.get("lot_type", "")).strip().upper(),
                total,
                available,
                updated,
                classify(available, total, high_min_pct, low_max_pct),
            )
            inconsistent += lot.anomalous
            lots.append(lot)
        if not lots:
            rejected += 0 if number in carparks else 1
            continue
        if number in carparks:
            merged += 1
            carparks[number].lots.extend(lots)
        else:
            carparks[number] = Carpark(number, lots)
    if not carparks:
        raise ValueError("no valid carpark records after validation")
    return MappedBatch(source, list(carparks.values()), rejected, merged, inconsistent)


@dataclass
class Summary:
    carparks: int
    total_lots: int
    available_lots: int
    average_pct: float
    full_or_low: int


def summarize(batch: MappedBatch, lot_type: str | None = None) -> Summary:
    """Dashboard.ApplyFilters summary: one lot per carpark (primary, or the chosen lot type)."""
    view: list[Lot] = []
    for cp in batch.carparks:
        if lot_type is None:
            view.append(cp.primary)
        else:
            view.extend(lot for lot in cp.lots if lot.lot_type == lot_type)
    valid = [lot for lot in view if not lot.anomalous]
    return Summary(
        carparks=len(view),
        total_lots=sum(lot.total for lot in valid),
        available_lots=sum(lot.available for lot in valid),
        average_pct=round(sum(lot.pct for lot in valid) / len(valid), 1) if valid else 0.0,
        full_or_low=sum(lot.status in (FULL, LOW) for lot in view),
    )
