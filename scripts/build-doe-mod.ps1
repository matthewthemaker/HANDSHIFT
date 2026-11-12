[CmdletBinding()]
param(
    [string]$OutputDirectory = (Join-Path $PSScriptRoot "..\dist"),
    [string]$Configuration = "Release"
)
$ErrorActionPreference = "Stop"
$root = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
$project = Join-Path $root "DoEMod\HANDSHIFT.DoE.Mod.csproj"

Write-Host "HANDSHIFT no-Unity build"
Write-Host "This builds a loader compatibility probe, not a finished Dungeons of Eternity hand-control mod."

if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    throw ".NET SDK not found. Install the .NET 6 SDK (or a compatible SDK capable of targeting net6.0), then rerun this script."
}
if (-not $env:HANDSHIFT_LEMONLOADER_DIR) {
    throw "Set HANDSHIFT_LEMONLOADER_DIR to the folder containing MelonLoader.dll from the exact LemonLoader release you installed."
}
$loaderDll = Join-Path $env:HANDSHIFT_LEMONLOADER_DIR "MelonLoader.dll"
if (-not (Test-Path $loaderDll)) {
    throw "Could not find $loaderDll"
}
if (-not $env:HANDSHIFT_GAME_PACKAGE) {
    throw "Set HANDSHIFT_GAME_PACKAGE to the installed DoE Android package ID. Do not guess it."
}
if (-not $env:HANDSHIFT_GAME_VERSION) {
    throw "Set HANDSHIFT_GAME_VERSION to the exact installed DoE version/build."
}

New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null
dotnet build $project -c $Configuration -p:LemonLoaderDir="$env:HANDSHIFT_LEMONLOADER_DIR"
if ($LASTEXITCODE -ne 0) { throw "Compilation failed. No package was created." }

$builtDll = Join-Path $root "DoEMod\bin\$Configuration\net6.0\HANDSHIFT.DoE.dll"
if (-not (Test-Path $builtDll)) { throw "Build reported success but the expected DLL was not found." }

$packageRoot = Join-Path $OutputDirectory "HANDSHIFT-DoE-Probe"
$modsDir = Join-Path $packageRoot "Mods"
New-Item -ItemType Directory -Force -Path $modsDir | Out-Null
Copy-Item $builtDll (Join-Path $modsDir "HANDSHIFT.DoE.dll") -Force
@{
    packageId = $env:HANDSHIFT_GAME_PACKAGE
    gameVersion = $env:HANDSHIFT_GAME_VERSION
    artifact = "HANDSHIFT.DoE.dll"
    status = "compatibility-probe-only"
    gameplayHooksEnabled = $false
} | ConvertTo-Json -Depth 4 | Set-Content (Join-Path $packageRoot "handshift-package.json") -Encoding UTF8
@"
HANDSHIFT DoE compatibility probe
Target package: $($env:HANDSHIFT_GAME_PACKAGE)
Target version: $($env:HANDSHIFT_GAME_VERSION)

This is not a hand-tracking gameplay mod. It only verifies that the loader can load a HANDSHIFT assembly and write a log entry.
Do not distribute this as a working DoE mod.
"@ | Set-Content (Join-Path $packageRoot "README.txt") -Encoding UTF8

$zip = Join-Path $OutputDirectory "HANDSHIFT-DoE-Probe.zip"
if (Test-Path $zip) { Remove-Item $zip -Force }
Compress-Archive -Path $packageRoot -DestinationPath $zip
Write-Host "Created $zip"
