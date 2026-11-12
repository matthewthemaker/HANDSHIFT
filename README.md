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

## Build status
Source project and CI workflow template are included. No APK is attached yet: this environment does not have Unity Editor, Android Build Support or a configured Unity license. See docs/QUEST_BUILD.md.

Do not patch your only copy of the game. Back up first. Do not distribute proprietary game APKs, assemblies or assets.
