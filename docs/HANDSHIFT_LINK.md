# HANDSHIFT Link

HANDSHIFT Link is a companion transfer system for moving HANDSHIFT-owned packages from a Windows PC to a Meta Quest headset without a USB cable.

## Important scope

HANDSHIFT Link transfers files and guides the user through installation. It does not bypass Android package-install consent, Quest security controls, game protections, or permissions. It does not silently patch Dungeons of Eternity.

The receiver must never modify the game unless a separately validated, version-specific installer is available and the user explicitly approves the operation.

## Transport strategy

Use a transport abstraction with two implementations:

1. **Bluetooth RFCOMM (experimental):** attempt an explicitly paired Bluetooth Classic serial/RFCOMM channel only when both Windows and the Quest receiver expose a compatible service. Quest OS Bluetooth discovery/pairing and app-level RFCOMM availability must be tested on real Quest 3 hardware before this is treated as supported.
2. **Local Wi-Fi transfer (preferred fallback):** transfer over a temporary authenticated local-network session. This does not require USB or internet, but the PC and Quest must be on the same LAN/hotspot. Pair with a short-lived one-time code and verify every package with SHA-256.

Do not claim Bluetooth is supported merely because the headset is paired in system settings. BLE is not a suitable general-purpose bulk APK transport; RFCOMM or local Wi-Fi is required.

## Package format

Use a signed/manifested HANDSHIFT bundle:
- manifest.json: schema version, product version, target headset family, target game identifier/version constraints, payload filenames, byte lengths, SHA-256 hashes, install action, minimum receiver version.
- payloads/: only HANDSHIFT-owned APKs, libraries, configuration, and documentation.
- No Dungeons of Eternity APK, assets, proprietary assemblies, or other game-owned files.

Reject path traversal, unexpected file names, oversized payloads, mismatched hashes, unsupported manifest versions, and packages not signed by a trusted HANDSHIFT release key. Never execute transferred files automatically.

## Quest receiver workflow

1. User opens HANDSHIFT Link Receiver.
2. User explicitly chooses Bluetooth or local Wi-Fi.
3. User pairs/authenticates the Windows sender with a one-time code.
4. Receiver accepts a bundle into app-private storage.
5. Receiver validates manifest, size, signature, and hashes.
6. Receiver presents the package contents and install steps.
7. Android/Quest system installer or a separately validated on-device patcher handles installation with explicit user consent.
8. Receiver records only non-sensitive install diagnostics; no camera frames or hand data are part of Link.

## Windows sender workflow

1. Discover/select an explicitly paired Quest receiver.
2. Authenticate with one-time code.
3. Select a HANDSHIFT release bundle.
4. Transfer with progress, cancellation, and retry.
5. Wait for receiver validation acknowledgement.
6. Show the next on-headset action; do not claim the game mod is installed until the receiver confirms the validated installer completed.

## Security and privacy

- Local transfer only; no cloud relay by default.
- Encrypt transport where supported and authenticate both endpoints.
- One-time pairing codes expire quickly and are single-use.
- User chooses every file and every install action.
- No silent installs, privilege escalation, rooting, or security bypasses.
- Never collect or transmit Quest camera data, hand tracking data, game files, or credentials.
- Keep diagnostic logs minimal and user-deletable.

## DoE compatibility gate

A transfer utility is not the DoE mod. Before enabling a DoE installation action, HANDSHIFT must have:
- verified current Quest package ID and game version;
- verified the current game's runtime/ABI and mod-loader compatibility;
- a tested, game-specific adapter;
- a recoverable backup/restore path where technically available;
- successful on-device test results on the target Quest 3 build.

Until those gates pass, Link may transfer the standalone HANDSHIFT QuestLab APK and diagnostic bundles, but must label them as QuestLab/testing components, not as a working DoE mod.

## Initial milestone

Build a transport-independent protocol and a Quest receiver proof-of-concept. Test Bluetooth RFCOMM discovery and pairing on Quest 3. If Quest OS or app permissions prevent RFCOMM, ship the same protocol over local Wi-Fi rather than pretending a Bluetooth-only solution works.
