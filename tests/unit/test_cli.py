from carpark_ops import cli


def test_validate_passes_on_repository(capsys):
    assert cli.main(["validate"]) == cli.OK
    assert "0 error(s)" in capsys.readouterr().out


def test_validate_strict_fails_on_warnings():
    assert cli.main(["validate", "--strict"]) == cli.FAILED  # disabled environments are warnings


def test_verify_oml_passes():
    assert cli.main(["verify-oml"]) == cli.OK


def test_deploying_personal_environment_is_refused_with_guidance(capsys):
    assert cli.main(["deploy", "--from", "personal", "--to", "test"]) == cli.USAGE
    assert "Service Studio" in capsys.readouterr().out


def test_plan_without_lifetime_credentials_is_a_usage_error(monkeypatch, capsys):
    monkeypatch.delenv("LIFETIME_URL", raising=False)
    monkeypatch.delenv("LIFETIME_TOKEN", raising=False)
    assert cli.main(["plan", "--from", "development", "--to", "test"]) == cli.USAGE
    assert "LIFETIME_URL" in capsys.readouterr().err


def test_health_on_disabled_environment_is_a_usage_error():
    assert cli.main(["health", "--env", "production"]) == cli.USAGE


def test_step_summary_is_written_in_ci(tmp_path, monkeypatch):
    summary = tmp_path / "summary.md"
    monkeypatch.setenv("GITHUB_STEP_SUMMARY", str(summary))
    cli._summary(["### Title", "line"])
    assert summary.read_text(encoding="utf-8") == "### Title\nline\n\n"
