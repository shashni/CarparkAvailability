# Creates .venv and installs carpark-ops with development tools. Usage: ./scripts/bootstrap.ps1 [-E2E]
param([switch]$E2E)
$ErrorActionPreference = 'Stop'
Set-Location (Split-Path $PSScriptRoot -Parent)

if (-not (Test-Path .venv)) { python -m venv .venv }
& .\.venv\Scripts\python.exe -m pip install --upgrade pip
$extras = if ($E2E) { '.[dev,e2e]' } else { '.[dev]' }
& .\.venv\Scripts\python.exe -m pip install -e $extras
if ($E2E) { & .\.venv\Scripts\python.exe -m playwright install chromium }

& .\.venv\Scripts\carpark-ops.exe validate
Write-Host "`nReady. Activate with: .\.venv\Scripts\Activate.ps1"
