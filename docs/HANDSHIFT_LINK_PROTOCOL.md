# HANDSHIFT Link protocol (v1 draft)

## Session

A sender and receiver establish a local session. Preferred first implementation is Wi-Fi TCP with TLS or an authenticated encrypted channel. Bluetooth RFCOMM is an experimental transport adapter; the application protocol remains identical.

Handshake fields:
- protocol: `HANDSHIFT-LINK`
- version: integer
- receiver_nonce: random 256-bit value
- sender_nonce: random 256-bit value
- pairing_code_proof: challenge response derived from a short-lived one-time code
- capabilities: supported transports, maximum bundle size, supported manifest versions

Do not send the pairing code itself over the transport. Use a standard password-authenticated key exchange or a carefully reviewed challenge-response design; do not invent cryptography for production. The proof-of-concept may use a one-time code over a TLS-protected local connection and must be marked development-only.

## Transfer

1. Sender sends manifest and declared bundle length.
2. Receiver validates schema, size ceiling, and allowed payload names before accepting bytes.
3. Transfer is chunked with monotonically increasing sequence numbers.
4. Receiver writes to a temporary app-private file, never to a game directory.
5. Receiver computes SHA-256 while receiving and compares it with the manifest.
6. Receiver atomically promotes a verified bundle into its inbox.
7. Receiver returns a validation result and waits for explicit user action.

## Required failure behavior

- Cancel: delete partial temporary files.
- Disconnect: retain no executable partial package; allow restart from zero in v1.
- Hash mismatch: delete temporary bundle and report failure.
- Unsupported target/game version: refuse installation and offer diagnostics only.
- Insufficient storage: refuse before transfer when possible.
- Invalid signature or unknown signing key: refuse installation.
- App crash: recover by deleting incomplete temporary files on next launch.

## Non-goals

- No game APK redistribution.
- No silent package installation.
- No root exploit, bootloader unlock, or privilege bypass.
- No injection into another process from the receiver app.
- No guarantee that Bluetooth RFCOMM is exposed by Quest OS.
