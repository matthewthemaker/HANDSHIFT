# Quest Lab scene wiring

The editor build entry point creates a minimal XR Origin and the HANDSHIFT tracker/visualizer.
Before building on a workstation, import Meta XR Core SDK and MRUK, configure the Android OpenXR loader and hand-tracking feature, and add Meta's passthrough building blocks.

Add the MRUK PassthroughCameraAccess component to the camera rig and assign that component to RealitySenseWorker. Add CameraPermissionGate and assign the worker. Enable passthrough in the app's OVR Manager configuration. The camera remains invisible; no preview object is created.

The Lab visualizer creates small world-space spheres at wrist, palm and fingertips to verify tracking. It is a development diagnostic only and must be disabled in the final in-game adapter.
