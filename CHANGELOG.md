# Changelog

All notable changes to this application and its infrastructure code. Format: [Keep a Changelog](https://keepachangelog.com/),
versions match the LifeTime application version tags.

## [Unreleased]

### Documentation
- README rewritten for reviewers: live URL, screenshots, architecture, decision records (D1–D10), data integrity
  rules, engineering practices, quality evidence, assumptions, limitations and trade-offs.
- `scripts/capture_screenshots.py` regenerates `docs/images/` from a deployed environment.

## [1.0.0] - 2026-10-22

### Added
- Dashboard: live availability from data.gov.sg with summary metrics, search (number, address, type), status,
  lot-type and minimum-lots filters, six sort orders, paging, detail popup with per-lot-type breakdown and HDB
  carpark information, explicit Live / Stale / Not refreshed / Unavailable states.
- Server: validated mapping (split-record merge, inconsistent-reading handling), 60 s snapshot cache with
  last-good fallback, daily HDB address sync, thresholds in site properties.
- `CarparkOps/Health` read-only REST endpoint for monitoring, drift detection and deployment verification.
- Infrastructure as code: `infra/` application and environment definitions with JSON Schemas, LifeTime
  promotion pipeline, `carpark-ops` CLI, unit/contract/smoke/e2e tests, GitHub Actions for CI, deploy,
  monitoring and upstream contract checks.

### Fixed (found by the verification suites before release)
- Carparks whose lot types data.gov.sg splits across two records were counted twice.
- Sorting and the detail view did nothing: System `ListSort`/`ListFilter` created through the Model API
  evaluate against `List.Current`. They were replaced by a JavaScript sort and a loop.
- First load could exceed the 10 s client timeout while the address cache synced; timeout raised to 60 s.
