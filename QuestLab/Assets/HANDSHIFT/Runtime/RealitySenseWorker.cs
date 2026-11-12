using System.Collections;
using UnityEngine;
using Meta.XR;

namespace HandShift.Quest
{
    /// Background-only camera processing. Never creates a preview or renderer.
    /// Requires MRUK PassthroughCameraAccess on Quest 3/3S.
    public sealed class RealitySenseWorker : MonoBehaviour
    {
        [SerializeField] private PassthroughCameraAccess cameraAccess;
        [SerializeField, Range(1, 10)] private int sampleEveryNFrames = 5;

        public bool IsRunning { get; private set; }
        private bool permissionGranted;
        private int frameCounter;

        public void SetPermissionGranted(bool granted)
        {
            permissionGranted = granted;
            if (!granted) StopProcessing();
            else if (isActiveAndEnabled) StartProcessing();
        }

        private void OnEnable() { if (permissionGranted) StartProcessing(); }
        private void OnDisable() => StopProcessing();

        private void StartProcessing()
        {
            if (IsRunning || cameraAccess == null || !permissionGranted) return;
            IsRunning = true;
            StartCoroutine(ProcessLoop());
        }

        private void StopProcessing()
        {
            IsRunning = false;
            StopAllCoroutines();
        }

        private IEnumerator ProcessLoop()
        {
            while (IsRunning)
            {
                yield return null;
                if (++frameCounter % sampleEveryNFrames != 0) continue;
                if (cameraAccess == null || !cameraAccess.IsPlaying) continue;

                // Obtain only an in-memory texture reference. No UI, material or renderer uses it.
                // Async GPU readback / native CV processing is the next implementation step.
                var frame = cameraAccess.GetTexture();
                if (frame == null) continue;
                yield return null;
            }
        }
    }
}
