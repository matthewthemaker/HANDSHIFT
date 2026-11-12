using UnityEngine;
#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine.Android;
#endif

namespace HandShift.Quest
{
    /// Requests only the headset camera permission. Camera access remains optional.
    public sealed class CameraPermissionGate : MonoBehaviour
    {
        [SerializeField] private RealitySenseWorker worker;
        private const string HeadsetCameraPermission = "horizonos.permission.HEADSET_CAMERA";

        private void Start()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (Permission.HasUserAuthorizedPermission(HeadsetCameraPermission))
                OnPermissionResult(true);
            else
                Permission.RequestUserPermission(HeadsetCameraPermission);
#else
            OnPermissionResult(false);
#endif
        }

        private void OnApplicationFocus(bool hasFocus)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (hasFocus && Permission.HasUserAuthorizedPermission(HeadsetCameraPermission))
                OnPermissionResult(true);
#endif
        }

        public void OnPermissionResult(bool granted)
        {
            if (worker != null) worker.SetPermissionGranted(granted);
        }
    }
}
