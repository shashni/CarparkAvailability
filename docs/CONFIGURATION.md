# Configuration

All runtime configuration is in OutSystems **site properties**, declared in `infra/application.json` and given
per-environment values in `infra/environments/<env>.json`. `carpark-ops validate` checks both against JSON Schema
(`infra/schemas/`) and the rules below. `carpark-ops drift` checks the deployed values.

## Site properties

| Property | Type | Default | Range | Effect |
|---|---|---|---|---|
| `HighAvailabilityMinPct` | Decimal | 50 | 0–100 | High availability when **more than** this % of lots is free |
| `LowAvailabilityMaxPct` | Decimal | 20 | 0–100 | Low availability **below** this % (with at least 1 free lot); Moderate in between |
| `StaleAfterMinutes` | Integer | 15 | 1–1440 | Data older than this is labelled Stale; the health status becomes `stale` |
| `ReadingOutdatedMinutes` | Integer | 60 | 1–10080 | A carpark reading this much older than the dataset is flagged "not recently updated" |
| `CacheTtlSeconds` | Integer | 60 | 5–3600 | Reuse a successful retrieval for this long |
| `MinRefreshIntervalSeconds` | Integer | 15 | 0–3600 | A user-forced refresh still reuses data younger than this |
| `CarparkInfoMaxAgeHours` | Integer | 24 | 1–720 | Re-sync interval for the HDB address cache |
| `HdbCarparkInfoResourceId` | Text | `d_23f946fa557947f93a8043bbef41dd09` | `d_` + 32 hex | data.gov.sg dataset id for HDB Carpark Information |
| `DataGovApiKey` | Text, **secret** | empty | | Optional data.gov.sg API key (sent as `x-api-key`) |

## Validation rules

Beyond types and ranges:

- `LowAvailabilityMaxPct` < `HighAvailabilityMinPct`
- `MinRefreshIntervalSeconds` ≤ `CacheTtlSeconds`
- `StaleAfterMinutes × 60` > `CacheTtlSeconds` (otherwise freshly cached data would be reported as stale)
- `checks.maxDataAgeMinutes` should not be looser than `StaleAfterMinutes` (warning)
- every declared property has a value in every environment, and no undeclared property appears
- secrets are never written inline; each has a `secrets` entry naming the environment variable that holds it
- an enabled environment contains no `REPLACE_ME` placeholders; LifeTime environments name their LifeTime environment
- the pipeline only uses LifeTime environments, never targets one environment twice, and requires approval for production
- the module's actual site properties (from `src/model/site-properties.cs`) match `infra/application.json`, name and type

## Environment file

```jsonc
{
  "name": "test",                       // must equal the file name
  "kind": "lifetime",                   // or "personal"
  "enabled": true,
  "baseUrl": "https://test.example.outsystemsenterprise.com",
  "lifetimeEnvironmentName": "Test",    // exact LifeTime name (lifetime kind only)
  "siteProperties": { "...": "..." },   // desired values, non-secret
  "secrets": { "DataGovApiKey": { "fromEnv": "CARPARK_DATAGOV_API_KEY", "required": false } },
  "checks": { "minCarparks": 1500, "requireLocationData": true, "maxDataAgeMinutes": 15 }
}
```

`checks` drive `verify`/`health`: the minimum plausible carpark count, whether the HDB address cache must be
populated, and the maximum acceptable data age.

Developer-only overrides can go in `infra/environments/<env>.local.json` (git-ignored, merged over the committed
file). Never put secrets there either.

## Secrets

| Name | Used by | Store |
|---|---|---|
| `LIFETIME_URL` / `LIFETIME_TOKEN` | `plan`, `deploy` | GitHub Actions secrets; `.env` locally |
| `CARPARK_DATAGOV_API_KEY` | contract/probe calls, drift check of `DataGovApiKey` | GitHub Actions secret; the module's `DataGovApiKey` site property in Service Center |

Rotate the LifeTime token by issuing a new service-account token, updating the secret and revoking the old one.
The health endpoint never returns the API key, only `dataGovApiKeyConfigured: true/false`.

## Changing configuration

1. Edit `infra/environments/<env>.json`, then run `carpark-ops validate`, open a PR, and merge.
2. Apply the value in Service Center for that environment.
3. `carpark-ops drift --env <env>` reports no drift.

To **add** a site property: add it in Service Studio, export the module (docs/DEPLOYMENT.md), declare it in
`infra/application.json` with a `healthField`, give it a value in every environment, and add it to the
`HealthReport` structure and `CarparkOps/Health` method so drift can see it.
