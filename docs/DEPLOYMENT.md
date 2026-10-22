# Deployment

## Environments

| Environment | Kind | Deployed by | Defined in |
|---|---|---|---|
| personal | OutSystems personal environment | Service Studio 1-Click Publish | `infra/environments/personal.json` |
| development | LifeTime-managed | developers publish from Service Studio | `infra/environments/development.json` |
| test | LifeTime-managed | `deploy.yml` / `carpark-ops deploy` | `infra/environments/test.json` |
| production | LifeTime-managed | `deploy.yml` / `carpark-ops deploy` (approval required) | `infra/environments/production.json` |

The three LifeTime environments ship **disabled** with `REPLACE_ME` hosts. To enable one, set `baseUrl` and
`lifetimeEnvironmentName` (exactly as named in LifeTime), set `"enabled": true`, and run `carpark-ops validate`.

## GitHub setup

One-time, in the GitHub repository settings:

1. **Secrets** (Settings → Secrets and variables → Actions):
   - `LIFETIME_URL`: e.g. `https://lifetime.yourcompany.outsystemsenterprise.com`
   - `LIFETIME_TOKEN`: a LifeTime **service account** token (LifeTime → User Management → Service Accounts)
     with *Change and Deploy Applications* on the CarparkAvailability application in Development, Test and Production.
   - `CARPARK_DATAGOV_API_KEY` (optional): only if you use a data.gov.sg API key.
2. **Environments** (Settings → Environments): create `test` and `production`. On `production` add required
   reviewers, and optionally a wait timer and a branch restriction to `main`.
3. **Branch protection** on `main`: require the `CI / Lint, validate infra, unit tests` check and one review.

## Releasing through LifeTime

1. Develop and publish in **development** from Service Studio.
2. Export the module into the repository and open a PR (see [Exporting the module](#exporting-the-module)).
   CI validates it.
3. After merge, run **Actions → Deploy (LifeTime) → Run workflow** with `from=development`, `to=test`,
   `version=1.x.y`, notes, and **dry run = true**. Read the plan in the job summary.
4. Run it again with dry run off. The workflow:
   - validates `infra/` and runs the unit tests;
   - tags version `1.x.y` from the modules running in Development (when Development has untagged changes);
   - creates the LifeTime deployment, aborts and deletes it if LifeTime reports conflicts, then starts it and
     polls until it finishes (45 min timeout);
   - runs `carpark-ops verify --env test` (health, freshness, drift, rules parity), then the smoke and browser
     suites.
5. Repeat with `from=test`, `to=production`. The deploy job pauses for the `production` environment approval.
   Production receives the **tagged** version from Test; untagged changes in Test are not promoted (the plan warns).

Only one deployment per target runs at a time (`concurrency: deploy-<target>`; a running deployment is never
cancelled).

Equivalent local commands (with `LIFETIME_URL`/`LIFETIME_TOKEN` in `.env` or the shell):

```bash
carpark-ops plan   --from development --to test --version 1.4.0 --notes "Sort fix"
carpark-ops deploy --from development --to test --version 1.4.0 --notes "Sort fix"
carpark-ops verify --env test
```

### Site properties after a deployment

LifeTime does not deploy site property values and its API cannot set them. For a **new** environment, or when a
release adds a site property:

1. Service Center → Factory → Modules → CarparkAvailability → Site Properties: set each value from
   `infra/environments/<env>.json`, and the secret `DataGovApiKey` from your secret store.
2. Run `carpark-ops drift --env <env>`. It must report *No drift*. The deploy workflow's verify job and the
   30-minute monitor fail while drift exists.

## Personal environment

1. Open `src/oml/CarparkAvailability.oml` in Service Studio (File → Open), connected to the personal environment.
2. Click **1-Click Publish**.
3. Run `carpark-ops verify --env personal`, or Actions → Monitor environments → Run workflow with `env=personal`.
4. If verify reports drift, fix the value in Service Center and verify again.

Make sure the Dashboard is the default screen (Service Studio → MainFlow → right-click Dashboard → Set as Default
Screen) so the application root URL opens it.

## Exporting the module

Do this whenever the module changes, so the repository stays the source of truth:

1. In Service Studio, with the module open, use **File → Save As** and overwrite
   `src/oml/CarparkAvailability.oml`.
2. Refresh the text snapshots in `src/model/`. They are Model API renderings produced by the Service Studio MCP
   read tools (`getDataModel`, `getStructures`, `getIntegrations`, `getSettings`, `getServerAction` for each action,
   `getScreen Dashboard`, `getTheme CarparkAvailability`, and `runQuery Root { Name References { Name } }` for
   `inventory.json`). Save each raw result as `src/model/<name>.txt`, then run `carpark-ops normalize-model`.
3. `carpark-ops update-manifest --exported-from development` (or `personal`).
4. `carpark-ops validate && pytest`, update `CHANGELOG.md`, commit and open a PR.

`validate` fails if the `.oml` or any snapshot changes without step 3, and if the module's site properties no
longer match `infra/application.json`.

## Rollback

- **LifeTime:** run the Deploy workflow (or `carpark-ops deploy`) from the environment that still runs the
  previous good version (e.g. `test` → `production` before Test is updated). Alternatively, in LifeTime open the
  application, choose the previous tagged version and deploy it. Then run `carpark-ops verify --env production`.
- **Personal:** check out the previous commit's `src/oml/CarparkAvailability.oml`
  (`git checkout <sha> -- src/oml/CarparkAvailability.oml`), publish it from Service Studio, verify.
- **Data:** nothing to roll back. The snapshot and address cache are rebuilt from data.gov.sg automatically.

## Reproducibility

A release is fully described by one commit: the `.oml` (SHA-256 pinned in `src/module.manifest.json`), the desired
configuration for every environment, the pipeline definition and the tooling version (`pyproject.toml`). The
platform dependencies (OutSystems UI, Charts, Maps, Users) are recorded in the manifest's inventory; LifeTime
deploys the versions present in the source environment.
