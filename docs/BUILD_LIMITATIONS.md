# Build limitations

The source and build entry point are present, but this execution environment does not include Unity Editor, Android Build Support, Meta XR SDK packages, a Quest device or a configured Unity license. Therefore no APK has been compiled or device-tested here.

Before a build:
- Open QuestLab in Unity 6000.0.66f2 or a compatible Unity 6 release.
- Install Meta XR Core SDK and MRUK from Meta's official package source.
- Configure XR Plug-in Management for Android and enable OpenXR hand tracking.
- Add the MRUK PassthroughCameraAccess component to the camera rig and assign it to RealitySenseWorker.
- Configure camera permission through Meta's documented permission flow.
- Confirm the Lab scene builds and launches on a real headset.

The GitHub Actions workflow requires Unity license secrets: UNITY_LICENSE, UNITY_EMAIL and UNITY_PASSWORD. The Lab APK is separate from the Dungeons of Eternity mod and cannot inject into the game.
