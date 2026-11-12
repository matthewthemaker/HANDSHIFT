using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;

namespace HandShift.Quest
{
    /// Background-only camera processing. Never creates a preview or renderer.
    /// Add Meta XR MRUK PassthroughCameraAccess and assign it in the Inspector.
    public sealed class RealitySenseWorker : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour cameraAccessComponent;
        [SerializeField, Range(1, 10)] private int sampleEveryNFrames = 5;
        public bool IsRunning { get; private set; }
        public event Action<float> BrightnessEstimateAvailable;
        private bool permissionGranted;
        private bool readbackPending;
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
                if (++frameCounter % sampleEveryNFrames != 0 || readbackPending) continue;
                if (cameraAccessComponent == null) continue;
                if (!(bool)isPlayingProperty.GetValue(cameraAccessComponent)) continue;
                var frame = getTextureMethod.Invoke(cameraAccessComponent, null) as Texture;
                if (frame == null) continue;
                readbackPending = true;
                AsyncGPUReadback.Request(frame, 0, TextureFormat.RGBA32, OnReadback);
            }
        }

        private void OnReadback(AsyncGPUReadbackRequest request)
        {
            readbackPending = false;
            if (request.hasError) return;
            var pixels = request.GetData<Color32>();
            if (pixels.Length == 0) return;
            double total = 0;
            for (int i = 0; i < pixels.Length; i++)
            {
                var p = pixels[i];
                total += (0.2126 * p.r + 0.7152 * p.g + 0.0722 * p.b) / 255.0;
            }
            BrightnessEstimateAvailable?.Invoke((float)(total / pixels.Length));
        }
    }
}
