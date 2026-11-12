# HANDSHIFT Link — Quest 3 feasibility checklist

## Bluetooth permission gate

Meta Horizon OS documentation (updated November 26, 2025) lists `BLUETOOTH_CONNECT`, `BLUETOOTH_SCAN`, and `BLUETOOTH_ADVERTISE` as permissions that require case-by-case review. Meta's stated permitted use is interoperability with a third-party Bluetooth device that cannot otherwise be paired through the Meta Quest app Bluetooth interface.

Therefore:
- Do not assume a Quest receiver app can freely scan for or pair with a Windows PC.
- Prefer a user-initiated system pairing flow if Quest OS exposes a supported route.
- Before implementing Bluetooth discovery, verify the exact Quest 3 OS version, available pairing UI, runtime permissions, and whether Meta permits this PC-to-headset use case.
- If the required permissions are unavailable or rejected, use local Wi-Fi with an explicit pairing code as the supported transport.

## Protocol feasibility

Android supports secure RFCOMM server/client sockets with a shared service UUID, and Android 12+ requires runtime `BLUETOOTH_CONNECT` permission for paired-device communication. This proves the Android platform has a Bluetooth transport API; it does not prove that the Quest 3 system image exposes every required capability to third-party apps.

## Installation boundary

HANDSHIFT Link can transfer an APK or mod bundle into receiver-private storage. The receiver can then ask Android to open the system package installer for an APK, which requires the user to approve installation and may require allowing that app as an install source.

This is separate from modifying Dungeons of Eternity. A normal receiver APK cannot silently patch another installed app or inject into its process. DoE patching requires a separately validated on-device mod-loader path and user consent.

## Go/no-go test on Quest 3

1. Confirm Quest OS version.
2. Confirm Windows PC can be paired from Quest system Bluetooth settings, if that pairing flow is offered.
3. Install a minimal receiver build with only the permissions Meta permits.
4. Test a secure RFCOMM connection and transfer a harmless text file.
5. Test a small HANDSHIFT-owned test APK transfer and explicit system-installer handoff.
6. Only then implement the full release bundle workflow.

No test result should be marked passed until observed on physical Quest 3 hardware.
