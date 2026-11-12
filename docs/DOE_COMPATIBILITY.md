# Dungeons of Eternity compatibility research

## Confirmed
- Dungeons of Eternity is a Unity VR game.
- The developer replied on Steam on 12 September 2025: "No official mod support yet."
- Steam update notes dated 21 September 2026 report finger tracking, rebindable SteamVR input and experimental OpenXR support for Steam Frame / PCVR. This is not evidence of arbitrary hand input support in the standalone Quest build.

## Quest candidate route (unverified)
LemonLoader is an Android-focused MelonLoader fork with documented IL2CPP and arm64-v8a support. That alone does not prove compatibility with this game. We need the exact Quest package ID, version/build, IL2CPP metadata, native libraries and a successful loader launch test.

If the exact build is verified:
1. Back up the original game and confirm it launches cleanly.
2. Follow the official LemonLoader installation process.
3. Confirm the patched game launches without HANDSHIFT.
4. Inspect the game's runtime and XR initialization.
5. Confirm hand tracking is requested and exposed through an approved Meta/OpenXR API.
6. Implement against actual game-side APIs and exact version metadata.
7. Test offline/single-player first; do not compromise multiplayer fairness or bypass security controls.

## Hard boundaries
A companion APK cannot normally read another app's camera stream, inspect its private Unity scene, write arbitrary virtual controller state, or inject into a sandboxed game. QuestLab can prove headset APIs inside HANDSHIFT itself; it cannot silently make Dungeons of Eternity consume those poses.

## Adapter contract
The game adapter must detect exact game version/architecture, fail closed on unknown versions, use permitted in-process hand data, map only to supported actions, and provide a complete disable/uninstall path.

Status: Quest standalone mod is research/unverified. No compatibility claim until tested on a physical Quest.
