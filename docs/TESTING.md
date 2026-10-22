# Testing

```bash
pytest                                                    # unit (default; offline)
pytest -m contract tests/contract                         # live data.gov.sg
CARPARK_ENV=personal pytest -m smoke tests/smoke          # live deployed app
CARPARK_ENV=personal pytest -m e2e tests/e2e              # browser (pip install -e .[e2e]; playwright install chromium)
pytest --cov=carpark_ops --cov-report=term-missing        # coverage
```

The default `pytest` run deselects the live markers, so it never needs network access or credentials.

## Unit tests (`tests/unit/`)

| File | Covers |
|---|---|
| `test_rules.py` | status bands at every boundary, configurable thresholds, split-record merging, main-lot selection, inconsistent readings, rejected records, summaries, unusable batches. Expectations are derived from a real trimmed payload (`fixtures/availability_sample.json`) and hand-written edge cases |
| `test_config.py` | the committed `infra/` is valid; each cross-field rule, inline-secret rejection, unknown properties, ranges, patterns, placeholders, pipeline approval rules, local overrides, secret resolution |
| `test_lifetime_client.py` | bearer auth, request bodies for versions and deployments, 202 start, retries on 429/5xx only, retry exhaustion, polling to a terminal status, timeout, module-version-key extraction |
| `test_deploy.py` | tag-or-reuse version logic, version format, stage validation, refusal of disabled/personal environments, conflict discard, every failed terminal status, plan text |
| `test_health.py` | default filling for omitted REST fields, health checks, drift per property, secret presence/required checks, refresh flag |
| `test_model.py` | the committed `.oml` signature and checksum, tamper detection, snapshot integrity, module ↔ infra site-property consistency, inventory, snapshot normalisation |
| `test_contracts_offline.py` | recorded upstream samples and the health sample satisfy the contracts; contract violations are detected |
| `test_cli.py` | exit codes, `--strict`, refusal to "deploy" a personal environment, missing credentials, job summaries |
| `test_workflows.py` | workflows parse, use read-only permissions and pinned actions, never interpolate inputs into shell, and match the pipeline |

## Live suites

- **Contract** (`tests/contract`): data.gov.sg availability and HDB datasets still match the schemas in
  `carpark_ops/schemas/`, the upstream timestamp is current, and the payload maps to a plausible batch.
- **Smoke** (`tests/smoke`): the dashboard shell is served by the right module; health is `ok` and matches its
  contract; no configuration drift; the carpark count matches the reference rules on the same live data; the
  health response exposes no secrets.
- **End-to-end** (`tests/e2e`, Playwright): live data loads and the summary count matches health; the default sort
  is descending by available lots; search plus keyboard-opened detail view; refresh sends exactly one request
  when double-clicked; no horizontal scroll and a single-column filter layout at 375 px.

## Why a reference implementation?

The business logic lives in OutSystems flows, which cannot be unit-tested from this repository.
`carpark_ops/rules.py` restates the rules in Python; unit tests pin them, and the smoke suite and `verify` compare
the deployed app with them on the same live payload. This is how the split-record defect and a no-op sort were
caught during development.
