# Build the HANDSHIFT Quest Lab APK

## Target hardware
- Hand-tracking lab: Quest devices supported by the installed Meta XR/OpenXR provider.
- RealitySense camera: Quest 3 or Quest 3S, Horizon OS v74+, passthrough enabled.

## Workstation
1. Install Unity 6 (Meta's current PCA sample documents Unity 6000.0.66f2 or later).
2. Add Android Build Support, Android SDK/NDK and OpenJDK through Unity Hub.
3. Install Meta XR Core SDK and Interaction SDK using Meta's official package instructions.
4. Install Mixed Reality Utility Kit (MRUK) for PassthroughCameraAccess.
5. Install Unity XR Hands and configure Meta OpenXR hand tracking.
6. Enable Developer Mode and connect with Meta Quest Developer Hub or ADB.

Open QuestLab in Unity. Resolve packages, import Meta XR packages, switch to Android, configure the OpenXR loader and add the HANDSHIFT scene to Build Settings.

## Hand tracking
Enable hands in Meta XR project settings. Choose Hands Only for a controller-free test, or Controllers and Hands while developing fallback. The included tracker reads Unity XR Hands joints and does not fabricate data when tracking is lost.

## Camera
Use MRUK PassthroughCameraAccess. Enable passthrough and camera access on the rig, request horizonos.permission.HEADSET_CAMERA, and start only after permission is granted. Never assign the camera texture to a UI, material, renderer or preview.

Meta's prerequisites: Quest 3/3S, Horizon OS v74+, passthrough enabled and camera permission.

## Build
- Android target, ARM64, IL2CPP.
- Package name: com.handshift.questlab.
- Sign with a development keystore for sideload testing.

## APK status
No signed APK is attached yet. A valid APK must be built in Unity and tested on hardware. This environment has no Unity Editor, Android Build Support or configured Unity license. Do not label an unbuilt artifact as installable.
