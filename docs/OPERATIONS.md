# Operations runbook

## Signals

| Signal | Where |
|---|---|
| `GET /CarparkAvailability/rest/CarparkOps/Health` | JSON: `status` (`ok` / `stale` / `no-data`), data age, counters, effective configuration |
| *Monitor environments* workflow | every 30 min; fails on stale data, low carpark count, missing addresses, drift, or parity mismatch |
| *Upstream contract* workflow | daily; fails when data.gov.sg changes shape |
| Service Center → Monitoring → General / Integrations, module `CarparkIntegration` | refresh summaries, upstream failures, address sync results |

`curl https://<host>/CarparkAvailability/rest/CarparkOps/Health?Refresh=true` loads through the normal cache
first. It never bypasses the upstream rate protection, so it is safe to call from monitors.

OutSystems omits zero/false values from REST responses: a missing `rejectedCount` means 0.

## Alerts and responses

### Health `status: stale` / dashboard shows "Stale data"

1. `carpark-ops health --env <env> --refresh`. If it now reports `ok`, the data was only stale because nobody
   had loaded the page; no action needed.
2. Still stale: check whether data.gov.sg itself is stale:
   `carpark-ops probe-source --env <env>` (or `pytest -m contract`), and compare the upstream `timestamp`.
3. If upstream is stale, it's an external incident: users already see the Stale banner. Nothing to fix locally.
4. If upstream is fresh but the app is not: Service Center → Monitoring → Errors, filter module
   `CarparkAvailability`; look for `CarparkDataUnavailable` or REST errors in `Carpark availability refresh failed:`
   log lines.

### Dashboard shows "Not refreshed" / health `status` ok but users report failures

The server is falling back to the last snapshot (labelled Cached). Same diagnosis as above, from step 2.
Typical causes: data.gov.sg outage, HTTP 429 rate limit (set an API key, `DataGovApiKey`), or a changed response
shape (the contract workflow will be failing too).

### Health `status: no-data`

No retrieval has ever succeeded in this environment. Check outbound HTTPS from the environment to
`api.data.gov.sg` and `data.gov.sg`, then the errors above.

### `hasLocationData: false` / addresses missing

The HDB Carpark Information sync failed. The dashboard still works and shows "Address not available".
Check the `CarparkIntegration` log for `HDB carpark info sync failed` or `sync skipped`. Verify the dataset id
(`HdbCarparkInfoResourceId`) on data.gov.sg. The sync retries automatically on the next load after
`CarparkInfoMaxAgeHours`. To force it sooner, temporarily set `CarparkInfoMaxAgeHours` to 1.

### Configuration drift

`carpark-ops drift --env <env>` lists each property with its expected and deployed values. Either set the value in
Service Center (the usual fix) or, if the deployed value is the intended one, change `infra/` through a PR.

### Rules parity mismatch

The app's carpark count differs from the reference implementation on the same live data by more than 1% (minimum 5).
A deployment changed the mapping logic or the feed changed shape. Compare `mergedRecordCount`,
`rejectedCount` and `inconsistentLotCount` with `carpark-ops probe-source`; roll back if a release caused it
(docs/DEPLOYMENT.md#rollback).

### Deployment stuck or failed

- `needs_user_intervention`: open the deployment in LifeTime, read the message (often a confirmation or an
  outdated dependency), resolve it, continue it there. The workflow leaves it running.
- `finished_with_errors` / `aborted`: read the deployment log in LifeTime and the workflow summary. The previous
  version keeps running; fix the cause and redeploy.
- Conflicts: the tool discards the deployment automatically and reports the conflicts. Resolve them in LifeTime
  (usually a dependency that must be deployed together), then redeploy.

## Routine tasks

| Task | How |
|---|---|
| Rotate the LifeTime token | new service-account token → update `LIFETIME_TOKEN` secret → revoke the old one |
| Add/rotate the data.gov.sg API key | update `DataGovApiKey` in Service Center and the `CARPARK_DATAGOV_API_KEY` secret |
| Change a threshold | edit `infra/`, PR, apply in Service Center, `carpark-ops drift` |
| Check what is deployed | `carpark-ops health --env <env> --json` |
| Dependency updates | Dependabot PRs (Python packages, GitHub Actions) weekly |

## Capacity and limits

- One upstream call per `CacheTtlSeconds` per front-end server, however many users there are; forced refreshes
  are capped by `MinRefreshIntervalSeconds`.
- The snapshot is one row (about 1 MB JSON); the address cache is about 2,300 rows replaced daily.
- Browser payload is about 1 MB per page load; filtering, sorting and paging run client-side.
