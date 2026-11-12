# HANDSHIFT no-Unity build path

HANDSHIFT now has a command-line C# build path for a **loader compatibility probe**. It does not require Unity Hub or the Unity Editor.

## What this build does
- Compiles a small MelonLoader mod assembly against the user's locally supplied `MelonLoader.dll`.
- Packages the DLL and target metadata into `dist/HANDSHIFT-DoE-Probe.zip`.
- Offers an optional ADB copy script for a Quest with USB debugging authorized.

## What it does not do
- It is not a finished Dungeons of Eternity hand-tracking mod.
- It does not patch or redistribute the game APK.
- It does not access hand joints, create virtual controller input, hook weapons/movement, or alter multiplayer.
- It does not establish that LemonLoader works with the current DoE Quest build.

## Requirements
- Windows PowerShell 5.1 or PowerShell 7.
- .NET 6 SDK (or compatible SDK that can target net6.0).
- A locally installed, exact LemonLoader release containing `MelonLoader.dll`.
- Exact DoE Android package ID and version/build, collected from the user's own installation.
- For optional install script: Android platform-tools and an authorized USB debugging connection.

## Build
Set environment values in PowerShell (replace placeholders with verified values):
```powershell
$env:HANDSHIFT_LEMONLOADER_DIR = "C:\Path\To\LemonLoader\Assemblies"
$env:HANDSHIFT_GAME_PACKAGE = "your.verified.package.id"
$env:HANDSHIFT_GAME_VERSION = "your.verified.game.version"
.\scripts\build-doe-mod.ps1
```

The script stops if required information or loader references are missing. It never emits a fake or empty DLL.

## Install
After a successful build and only after confirming the loader is installed for this exact app:
```powershell
.\scripts\install-doe-mod.ps1
```
The install script uses ADB over USB, not Bluetooth. The current user previously had an empty ADB device list, so this step may remain unavailable until USB authorization is fixed.

## Next engineering gate
Before gameplay hooks can be written, capture and verify the exact DoE package ID/version, runtime/architecture, generated IL2CPP interop assemblies and loader startup log. Do not guess game class names or input APIs. Unknown builds must fail closed.
