using System.Collections;
using System.Reflection;
using UnityEngine;

namespace HandShift.Quest
{
    /// Background-only camera processing. Never creates a preview or renderer.
    /// Add Meta XR MRUK PassthroughCameraAccess and assign it in the Inspector.
    public sealed class RealitySenseWorker : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour cameraAccessComponent;
        [SerializeField, Range(1, 10)] private int sampleEveryNFrames = 5;
        public bool IsRunning { get; private set; }
        private bool permissionGranted;
        private int frameCounter;
        private PropertyInfo isPlayingProperty;
        private MethodInfo getTextureMethod;

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
            if (IsRunning || cameraAccessComponent == null || !permissionGranted) return;
            var type = cameraAccessComponent.GetType();
            isPlayingProperty = type.GetProperty("IsPlaying");
            getTextureMethod = type.GetMethod("GetTexture", BindingFlags.Instance | BindingFlags.Public);
            if (isPlayingProperty == null || getTextureMethod == null) return;
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
                if (cameraAccessComponent == null) continue;
                if (!(bool)isPlayingProperty.GetValue(cameraAccessComponent)) continue;
                // Texture is held in memory only; no UI, material, renderer or preview references it.
                // Async GPU readback / native CV analysis is the next implementation step.
                var frame = getTextureMethod.Invoke(cameraAccessComponent, null) as Texture;
                if (frame == null) continue;
                yield return null;
            }
        }
    }
}
