# HANDSHIFT Dungeons of Eternity Quest mod candidate

No verified official mod API has been identified. The developer publicly stated on 12 September 2025 that the game had no official mod support.

LemonLoader is a possible Android Unity IL2CPP mod-loader research route, but compatibility is app-specific and not guaranteed. The exact Quest game build must be tested.

A valid plugin requires exact target game IL2CPP metadata / generated interop assemblies and the loader API. Those proprietary game assemblies are not included. Guessing types or emitting a dummy DLL would be misleading and may crash the game.

Required evidence:
- exact Quest package name and game version/build;
- original APK split metadata and arm64 libraries;
- proof LemonLoader patches and launches that exact build;
- manifest permission status for hand tracking and camera;
- confirmation the game process initializes a supported hand-tracking provider;
- legitimate action hooks for movement and weapons.

Back up the game before patching. Never distribute the game's APK or proprietary assemblies.
