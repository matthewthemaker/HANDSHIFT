# HANDSHIFT — Meta Quest prototype

Target: Meta Quest standalone, Dungeons of Eternity first.

This repository separates two deliverables:
1. QuestLab: a Unity/OpenXR test app for verifying hand joints, calibration and optional RealitySense camera processing.
2. DoE-Mod: an in-process Android/IL2CPP mod candidate, only if the exact game build, loader, permissions and supported integration points are verified.

Important: QuestLab is not a game overlay or controller emulator. A normal third-party Quest app cannot inject hand poses into another sandboxed app or read its private camera stream. The actual game integration would need to run in the game process through a legitimate mod route or official API.

## Research findings
- Meta supports Quest hand tracking through its app SDKs and OpenXR.
- Meta's hand-pose data use policy limits the data to enabling hand tracking within the app. Sending data into a separate app/process requires policy review. The intended integration must be in-process and compliant.
- Passthrough Camera API is supported on Quest 3/3S with Horizon OS v74+, passthrough enabled and the HEADSET_CAMERA permission (or broader CAMERA permission).
- Othergate stated on 12 September 2025 that Dungeons of Eternity had no official mod support.
- A September 2026 Steam update added finger tracking, rebindable SteamVR input and experimental OpenXR support for Steam Frame/PCVR. This does not prove Quest standalone mod support.
- LemonLoader is a possible Android Unity IL2CPP research route, not a compatibility guarantee.

## Layout
- QuestLab: Unity 6 Android/Quest prototype source.
- DoE-Mod: game-specific research and adapter boundary.
- docs: build, compatibility and privacy notes.

## No-Unity build path (new)
A command-line C# project now exists under `DoEMod/`, with PowerShell build and optional ADB-copy scripts under `scripts/`. It does not require Unity Hub or the Unity Editor. The first output is deliberately a **LemonLoader compatibility probe**, not a functional hand-tracking gameplay mod. It requires the exact local LemonLoader assembly plus verified DoE package ID and version. See `docs/NO_UNITY_BUILD.md`.

The original `QuestLab/` remains a separate Unity-based research prototype. No finished DoE gameplay mod or tested APK is attached yet. Do not treat the probe as a working mod.

Do not patch your only copy of the game. Back up first. Do not distribute proprietary game APKs, assemblies or assets.
