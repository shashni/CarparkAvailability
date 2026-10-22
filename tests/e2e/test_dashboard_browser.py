"""Browser tests (Playwright). Run: pip install -e .[e2e] && playwright install chromium && pytest -m e2e"""

import os
import re

import pytest

from carpark_ops import config, health

pytestmark = pytest.mark.e2e
playwright = pytest.importorskip("playwright.sync_api")
ENV_NAME = os.environ.get("CARPARK_ENV", "personal")


@pytest.fixture(scope="module")
def env():
    environment = config.load_environment(ENV_NAME)
    if not environment.enabled:
        pytest.skip(f"environment {ENV_NAME} is disabled")
    return environment


@pytest.fixture(scope="module")
def browser():
    with playwright.sync_playwright() as p:
        b = p.chromium.launch()
        yield b
        b.close()


def open_dashboard(browser, env, viewport):
    page = browser.new_page(viewport=viewport)
    page.goto(env.url(config.load_application()["paths"]["dashboard"]))
    page.wait_for_selector(".cp-card", timeout=90_000)
    return page


def numbers(page):
    return [int(t.replace(",", "")) for t in page.locator(".cp-avail-number").all_inner_texts() if t.strip() != "–"]


def test_loads_live_data_with_matching_count(browser, env):
    page = open_dashboard(browser, env, {"width": 1440, "height": 900})
    assert page.locator(".cp-fresh").inner_text() in ("Live data", "Stale data", "Not refreshed")
    assert re.search(r"Last updated: \d{1,2} \w{3} \d{4}", page.locator(".cp-updated").inner_text())
    shown = int(page.locator(".cp-kpi-value").first.inner_text().replace(",", ""))
    report = health.fetch_health(env, config.load_application()["paths"]["health"])
    assert abs(shown - report["carparkCount"]) <= max(5, shown // 100)


def test_default_sort_is_most_available_first(browser, env):
    page = open_dashboard(browser, env, {"width": 1440, "height": 900})
    values = numbers(page)
    assert values == sorted(values, reverse=True)


def test_search_and_detail_view(browser, env):
    page = open_dashboard(browser, env, {"width": 1440, "height": 900})
    first_id = page.locator(".cp-card-id").first.inner_text()
    page.fill(".cp-field-search input", first_id.lower())
    page.wait_for_function("document.querySelector('.cp-results-count').innerText.includes('of 1 carpark')")
    page.locator(".cp-card").first.focus()
    page.keyboard.press("Enter")
    page.wait_for_selector(".cp-detail-body .cp-lot-row")
    assert first_id in page.locator(".cp-detail-title").inner_text()


def test_refresh_is_single_flight(browser, env):
    page = open_dashboard(browser, env, {"width": 1440, "height": 900})
    calls = []
    page.on("request", lambda r: calls.append(r.url) if "ActionCarpark_GetAvailability" in r.url else None)
    button = page.locator(".cp-refresh-btn")
    button.click()
    button.click(force=True)
    page.wait_for_function("!document.querySelector('.cp-refresh-btn').disabled", timeout=90_000)
    assert len(calls) == 1


def test_mobile_layout_has_no_horizontal_scroll(browser, env):
    page = open_dashboard(browser, env, {"width": 375, "height": 812})
    assert page.evaluate("document.documentElement.scrollWidth <= window.innerWidth")
    assert (
        page.evaluate(
            "getComputedStyle(document.querySelector('.cp-filter-grid')).gridTemplateColumns.split(' ').length"
        )
        == 1
    )
