"""Captures the README screenshots from a deployed environment.

Usage: python scripts/capture_screenshots.py [--env personal]
Needs: pip install -e .[e2e] && playwright install chromium
Writes PNGs to docs/images/. Re-run after UI changes so the README never shows an outdated screen.
"""

from __future__ import annotations

import argparse
from pathlib import Path

from playwright.sync_api import Page, sync_playwright

from carpark_ops import config

OUT = Path(__file__).resolve().parents[1] / "docs" / "images"


def open_dashboard(page: Page, url: str) -> None:
    page.goto(url)
    page.wait_for_selector(".cp-card", timeout=90_000)
    page.wait_for_timeout(1200)  # let fonts and the freshness label settle


def shot(page: Page, name: str, **kwargs) -> None:
    path = OUT / f"{name}.png"
    page.screenshot(path=str(path), **kwargs)
    print(f"wrote {path.relative_to(OUT.parents[1])}")


def main() -> None:
    parser = argparse.ArgumentParser()
    parser.add_argument("--env", default="personal")
    args = parser.parse_args()
    env = config.load_environment(args.env)
    url = env.url(config.load_application()["paths"]["dashboard"])
    OUT.mkdir(parents=True, exist_ok=True)

    with sync_playwright() as p:
        browser = p.chromium.launch()

        desktop = browser.new_page(viewport={"width": 1440, "height": 900}, device_scale_factor=1)
        open_dashboard(desktop, url)
        shot(desktop, "dashboard-desktop")

        desktop.locator(".cp-results-head").scroll_into_view_if_needed()
        desktop.evaluate("document.querySelector('.cp-filters').scrollIntoView({block: 'start'})")
        desktop.wait_for_timeout(500)
        shot(desktop, "results-grid")

        desktop.fill(".cp-field-search input", "ang mo kio")
        desktop.select_option(".cp-field-sort select", label="Fewest available lots")
        desktop.wait_for_timeout(1000)
        desktop.evaluate("document.querySelector('.cp-filters').scrollIntoView({block: 'start'})")
        shot(desktop, "search-and-filters")

        desktop.fill(".cp-field-search input", "sk48")
        desktop.wait_for_function("document.querySelector('.cp-results-count').innerText.includes('of 1 carpark')")
        desktop.locator(".cp-card").first.click()
        desktop.wait_for_selector(".cp-detail-body .cp-lot-row")
        desktop.wait_for_timeout(600)
        shot(desktop, "detail-view")

        mobile = browser.new_page(viewport={"width": 390, "height": 844}, device_scale_factor=2, is_mobile=True)
        open_dashboard(mobile, url)
        shot(mobile, "dashboard-mobile")
        mobile.evaluate("document.querySelector('.cp-results-head').scrollIntoView({block: 'start'})")
        mobile.wait_for_timeout(500)
        shot(mobile, "results-mobile")

        browser.close()


if __name__ == "__main__":
    main()
