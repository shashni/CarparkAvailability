"""carpark-ops command line. Exit codes: 0 = passed, 1 = checks failed, 2 = usage or configuration error."""

from __future__ import annotations

import argparse
import json
import os
import sys
from pathlib import Path

import requests

from . import config, datagov, deploy, health, model, rules
from .lifetime import LifeTimeClient, LifeTimeError

OK, FAILED, USAGE = 0, 1, 2


def _summary(lines: list[str]) -> None:
    """Also writes to the GitHub Actions job summary when running in CI."""
    for line in lines:
        print(line)
    path = os.environ.get("GITHUB_STEP_SUMMARY")
    if path:
        with open(path, "a", encoding="utf-8") as fh:
            fh.write("\n".join(lines) + "\n\n")


def _print_issues(issues: list[config.Issue], strict: bool) -> int:
    errors = [i for i in issues if i.severity == "error"]
    warnings = [i for i in issues if i.severity == "warning"]
    for issue in errors + warnings:
        print(issue)
    failed = bool(errors) or (strict and bool(warnings))
    print(f"{len(errors)} error(s), {len(warnings)} warning(s)" + (" - FAILED" if failed else " - OK"))
    return FAILED if failed else OK


def _module(app: dict) -> dict:
    return app["modules"][0]


def model_issues(root: Path, app: dict) -> list[config.Issue]:
    """The committed module, its manifest, its text snapshots and infra/ must describe the same thing."""
    issues: list[config.Issue] = []
    module = _module(app)
    manifest_path = root / module["manifest"]
    if not manifest_path.is_file():
        return [config.Issue("error", module["manifest"], "missing; run `carpark-ops update-manifest`")]
    manifest = model.load_manifest(root, module)
    for problem in model.verify_oml(root / module["oml"], manifest):
        issues.append(config.Issue("error", module["oml"], problem))
    model_dir = root / "src" / "model"
    for name, digest in manifest.get("snapshots", {}).items():
        path = model_dir / name
        if not path.is_file():
            issues.append(config.Issue("error", "src/model", f"{name} is listed in the manifest but missing"))
        elif model.sha256_of(path) != digest:
            issues.append(config.Issue("error", "src/model", f"{name} changed without `carpark-ops update-manifest`"))
    if (model_dir / "site-properties.cs").is_file():
        in_model = model.site_properties_in_model(model_dir)
        declared = app["siteProperties"]
        type_map = {"Integer": "integer", "Decimal": "decimal", "Text": "text", "Boolean": "boolean"}
        for name in sorted(set(in_model) - set(declared)):
            issues.append(
                config.Issue("error", "application.json", f"module defines site property {name} but infra/ does not")
            )
        for name in sorted(set(declared) - set(in_model)):
            issues.append(
                config.Issue(
                    "error", "application.json", f"{name} is declared but the module has no such site property"
                )
            )
        for name in sorted(set(declared) & set(in_model)):
            if type_map.get(in_model[name]) != declared[name]["type"]:
                issues.append(
                    config.Issue(
                        "error",
                        "application.json",
                        f"{name} is {in_model[name]} in the module but {declared[name]['type']} in infra/",
                    )
                )
        refs = manifest.get("inventory", {}).get("references", [])
        for dep in app.get("platformDependencies", []):
            if refs and dep not in refs:
                issues.append(
                    config.Issue(
                        "error", "application.json", f"platform dependency {dep} is not referenced by the module"
                    )
                )
    return issues


# ---- commands ----------------------------------------------------------------------------------------------
def cmd_validate(args) -> int:
    root = config.repo_root()
    issues = config.validate_all(root)
    issues += model_issues(root, config.load_application(root))
    return _print_issues(issues, args.strict)


def cmd_verify_oml(args) -> int:
    root = config.repo_root()
    app = config.load_application(root)
    problems = model_issues(root, app)
    return _print_issues(problems, strict=True)


def cmd_normalize_model(args) -> int:
    written = model.normalize_export(config.repo_root() / "src" / "model")
    print("\n".join(f"normalised {p.name}" for p in written) or "nothing to normalise")
    return OK


def cmd_update_manifest(args) -> int:
    root = config.repo_root()
    app = config.load_application(root)
    module = _module(app)
    manifest = model.build_manifest(root, module, args.exported_from)
    path = root / module["manifest"]
    path.write_text(json.dumps(manifest, indent=2) + "\n", encoding="utf-8", newline="\n")
    print(f"wrote {path.relative_to(root)} (oml sha256 {manifest['oml']['sha256'][:12]}...)")
    return OK


def _lifetime_client() -> LifeTimeClient:
    url, token = os.environ.get("LIFETIME_URL", ""), os.environ.get("LIFETIME_TOKEN", "")
    if not url or not token:
        raise config.ConfigError("LIFETIME_URL and LIFETIME_TOKEN must be set (see .env.example)")
    return LifeTimeClient(url, token)


def _plan(args):
    root = config.repo_root()
    app, pipeline = config.load_application(root), config.load_pipeline(root)
    source, target = config.load_environment(args.source, root), config.load_environment(args.target, root)
    blocking = [i for i in config.validate_all(root) if i.severity == "error"]
    if blocking:
        raise config.ConfigError("infra/ has validation errors; run `carpark-ops validate`")
    client = _lifetime_client()
    notes = args.notes or f"carpark-ops: {args.source} -> {args.target}"
    plan = deploy.plan_promotion(client, app, pipeline, source, target, version=args.version, notes=notes)
    return client, pipeline, plan


def _refuse_personal(args) -> int | None:
    for name in (args.source, args.target):
        try:
            env = config.load_environment(name)
        except config.ConfigError:
            continue
        if env.kind == "personal":
            print(
                f"'{name}' is a personal environment: there is no deployment API. Open "
                "src/oml/CarparkAvailability.oml in Service Studio, 1-Click Publish, then run "
                f"`carpark-ops verify --env {name}`. See docs/DEPLOYMENT.md."
            )
            return USAGE
    return None


def cmd_plan(args) -> int:
    refused = _refuse_personal(args)
    if refused is not None:
        return refused
    _, _, plan = _plan(args)
    _summary(["### Deployment plan", "```", plan.describe(), "```"])
    return OK


def cmd_deploy(args) -> int:
    refused = _refuse_personal(args)
    if refused is not None:
        return refused
    client, pipeline, plan = _plan(args)
    _summary(["### Deployment plan", "```", plan.describe(), "```"])
    if args.dry_run:
        print("Dry run: nothing deployed.")
        return OK
    result = deploy.apply_plan(client, plan, pipeline)
    _summary([f"**Deployment {result.deployment_key}: {result.status}** {result.info}"])
    return OK if result.status in ("finished_successful", "finished_with_warnings") else FAILED


def _health_for(env_name: str, refresh: bool):
    root = config.repo_root()
    app = config.load_application(root)
    env = config.load_environment(env_name, root)
    if not env.enabled:
        raise config.ConfigError(f"environment '{env_name}' is disabled")
    return app, env, health.fetch_health(env, app["paths"]["health"], refresh=refresh)


def cmd_health(args) -> int:
    app, env, report = _health_for(args.env, args.refresh)
    if args.json:
        print(json.dumps(report, indent=2))
        return OK
    issues = health.health_issues(report, env, _module(app)["name"])
    print(
        f"{env.name}: status={report['status']} carparks={report['carparkCount']} "
        f"dataAge={report['dataAgeSeconds']}s source={report['sourceTimestamp']}"
    )
    return _print_issues(issues, strict=False)


def _secret_presence(env: config.Environment, app: dict) -> dict[str, bool]:
    """Only meaningful when the operator has the secret locally; otherwise unknown (skipped)."""
    presence: dict[str, bool] = {}
    for prop, spec in app["siteProperties"].items():
        if spec.get("secret") and os.environ.get(env.secrets.get(prop, {}).get("fromEnv", "")) is not None:
            presence[prop] = bool(config.resolve_secret(env, prop))
    return presence


def cmd_drift(args) -> int:
    app, env, report = _health_for(args.env, False)
    items = health.drift(report, app, env, _secret_presence(env, app))
    lines = [f"### Configuration drift: {env.name}"]
    lines += [f"- {item}" for item in items] or ["No drift: deployed site properties match infra/."]
    _summary(lines)
    if items:
        print("Fix: set these values in Service Center > Factory > Modules > CarparkAvailability > Site Properties.")
    return FAILED if items else OK


def cmd_verify(args) -> int:
    """Post-deployment verification: dashboard reachable, health OK, no drift, rules parity."""
    app, env, report = _health_for(args.env, True)
    lines = [f"### Verification: {env.name}"]
    failed = False
    page = requests.get(env.url(app["paths"]["dashboard"]), timeout=60)
    # A Reactive app serves a JavaScript shell; its base path proves the right module answered.
    shell_marker = f"basePath: '/{_module(app)['name']}/'"
    ok_page = page.status_code == 200 and shell_marker in page.text
    lines.append(f"- Dashboard page: {'OK' if ok_page else f'FAILED (HTTP {page.status_code})'}")
    failed |= not ok_page
    issues = health.health_issues(report, env, _module(app)["name"])
    errors = [i for i in issues if i.severity == "error"]
    lines.append(
        f"- Health: {report['status']}, {report['carparkCount']} carparks, data age "
        f"{report['dataAgeSeconds']}s, addresses {'yes' if report['hasLocationData'] else 'no'}"
    )
    lines += [f"  - {i}" for i in issues]
    failed |= bool(errors)
    items = health.drift(report, app, env, _secret_presence(env, app))
    lines.append(f"- Configuration drift: {'none' if not items else len(items)}")
    lines += [f"  - {i}" for i in items]
    failed |= bool(items)
    if not args.skip_parity:
        parity_lines, parity_failed = _parity(report, env)
        lines += parity_lines
        failed |= parity_failed
    lines.append("**Result: FAILED**" if failed else "**Result: passed**")
    _summary(lines)
    return FAILED if failed else OK


def _parity(report: dict, env: config.Environment) -> tuple[list[str], bool]:
    payload = datagov.fetch_availability(config.resolve_secret(env, "DataGovApiKey"))
    expected = rules.map_batch(payload, report["highAvailabilityMinPct"], report["lowAvailabilityMaxPct"])
    exp_count = len(expected.carparks)
    tolerance = max(5, exp_count // 100)  # the app and this check may read different minutes of the feed
    diff = abs(report["carparkCount"] - exp_count)
    ok = diff <= tolerance
    return (
        [
            f"- Rules parity: app {report['carparkCount']} carparks vs reference {exp_count} "
            f"(tolerance {tolerance}): {'OK' if ok else 'FAILED'}"
        ],
        not ok,
    )


def cmd_probe_source(args) -> int:
    root = config.repo_root()
    env = config.load_environment(args.env, root)
    key = config.resolve_secret(env, "DataGovApiKey")
    availability = datagov.fetch_availability(key)
    errs = datagov.contract_errors(availability, "availability.schema.json")
    batch = rules.map_batch(availability)
    print(
        f"availability: {len(errs)} contract error(s); {len(batch.carparks)} carparks, "
        f"{batch.merged} merged, {batch.rejected} rejected, {batch.inconsistent_lots} inconsistent lots"
    )
    resource = env.site_properties.get("HdbCarparkInfoResourceId")
    info = datagov.fetch_hdb_info(resource, api_key=key)
    errs2 = datagov.contract_errors(info, "datastore.schema.json")
    print(f"hdb info: {len(errs2)} contract error(s); {len(info.get('result', {}).get('records', []))} records")
    for e in errs + errs2:
        print("  " + e)
    return FAILED if errs or errs2 else OK


def build_parser() -> argparse.ArgumentParser:
    parser = argparse.ArgumentParser(prog="carpark-ops", description=__doc__)
    sub = parser.add_subparsers(dest="command", required=True)

    p = sub.add_parser("validate", help="validate infra/, the module package and its snapshots")
    p.add_argument("--strict", action="store_true", help="treat warnings as failures")
    p.set_defaults(func=cmd_validate)

    p = sub.add_parser("verify-oml", help="check the committed .oml against its manifest")
    p.set_defaults(func=cmd_verify_oml)

    p = sub.add_parser("normalize-model", help="clean raw MCP exports in src/model/")
    p.set_defaults(func=cmd_normalize_model)

    p = sub.add_parser("update-manifest", help="recompute src/module.manifest.json after an export")
    p.add_argument("--exported-from", required=True, help="where the module was exported from, e.g. 'personal'")
    p.set_defaults(func=cmd_update_manifest)

    for name, func, helptext in (
        ("plan", cmd_plan, "show what a LifeTime promotion would do"),
        ("deploy", cmd_deploy, "promote through LifeTime"),
    ):
        p = sub.add_parser(name, help=helptext)
        p.add_argument("--from", dest="source", required=True)
        p.add_argument("--to", dest="target", required=True)
        p.add_argument("--version", help="version tag when the stage creates one, e.g. 1.2.0")
        p.add_argument("--notes", help="deployment notes / change log")
        if name == "deploy":
            p.add_argument("--dry-run", action="store_true")
        p.set_defaults(func=func)

    for name, func, helptext in (
        ("health", cmd_health, "read the health endpoint"),
        ("drift", cmd_drift, "compare deployed site properties with infra/"),
        ("verify", cmd_verify, "full post-deployment verification"),
        ("probe-source", cmd_probe_source, "check data.gov.sg against the contracts"),
    ):
        p = sub.add_parser(name, help=helptext)
        p.add_argument("--env", required=True)
        if name == "health":
            p.add_argument("--refresh", action="store_true")
            p.add_argument("--json", action="store_true")
        if name == "verify":
            p.add_argument("--skip-parity", action="store_true")
        p.set_defaults(func=func)
    return parser


def main(argv: list[str] | None = None) -> int:
    args = build_parser().parse_args(argv)
    try:
        return args.func(args)
    except (config.ConfigError, deploy.DeploymentError) as exc:
        print(f"error: {exc}", file=sys.stderr)
        return USAGE
    except (LifeTimeError, requests.RequestException) as exc:
        print(f"error: {exc}", file=sys.stderr)
        return FAILED


if __name__ == "__main__":
    sys.exit(main())
