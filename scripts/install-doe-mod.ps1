[CmdletBinding()]
param([string]$PackageId = $env:HANDSHIFT_GAME_PACKAGE)
$ErrorActionPreference = "Stop"
if (-not $PackageId) { throw "Supply -PackageId or set HANDSHIFT_GAME_PACKAGE." }
if (-not (Get-Command adb -ErrorAction SilentlyContinue)) { throw "ADB not found. Install Android platform-tools and ensure adb is on PATH." }
$device = (& adb devices) | Select-String "\sdevice$"
if (-not $device) { throw "No authorized Quest detected. Enable USB debugging, connect USB, and accept the RSA prompt. This installer does not use Bluetooth." }
$source = Join-Path $PSScriptRoot "..\dist\HANDSHIFT-DoE-Probe\Mods\HANDSHIFT.DoE.dll"
if (-not (Test-Path $source)) { throw "Build the package first with scripts\build-doe-mod.ps1." }
$target = "/sdcard/MelonLoader/$PackageId/Mods"
& adb shell "mkdir -p '$target'"
if ($LASTEXITCODE -ne 0) { throw "Could not create the mod directory." }
& adb push $source "$target/HANDSHIFT.DoE.dll"
if ($LASTEXITCODE -ne 0) { throw "ADB push failed." }
Write-Host "Copied compatibility probe. This does not prove DoE compatibility or enable hand controls."
