#!/usr/bin/env bash
# Creates .venv and installs carpark-ops with development tools. Usage: ./scripts/bootstrap.sh [--e2e]
set -euo pipefail
cd "$(dirname "$0")/.."

[ -d .venv ] || python3 -m venv .venv
.venv/bin/python -m pip install --upgrade pip
if [ "${1:-}" = "--e2e" ]; then
  .venv/bin/python -m pip install -e ".[dev,e2e]"
  .venv/bin/python -m playwright install chromium
else
  .venv/bin/python -m pip install -e ".[dev]"
fi

.venv/bin/carpark-ops validate
echo
echo "Ready. Activate with: source .venv/bin/activate"
