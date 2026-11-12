# RealitySense privacy contract

- Ask permission before camera access.
- Process on-device only.
- Do not upload frames, thumbnails, room maps or derived environmental data.
- Do not display camera images or create a preview.
- Do not use camera images for face identification.
- Do not persist raw frames by default.
- If the user explicitly enables retention, encrypt with a platform-backed key and delete no later than seven days after capture.
- Provide immediate delete-all and camera-disable controls in the eventual settings.
- If permission is denied or processing exceeds its resource budget, disable RealitySense and continue without it.

Seven days is a maximum, not a target. Preferred raw-frame retention is zero.

The standalone Lab app cannot keep the game foreground while it runs. In a future in-process game mod, camera processing must use a bounded worker, drop stale frames and yield to the game's render/input threads.
