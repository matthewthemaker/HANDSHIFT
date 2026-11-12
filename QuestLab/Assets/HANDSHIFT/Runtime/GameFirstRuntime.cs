using UnityEngine;

namespace HandShift.Quest
{
    /// Lab app status only. The final game integration must execute in-process.
    public sealed class GameFirstRuntime : MonoBehaviour
    {
        [SerializeField] private HandShiftHandTracker handTracker;
        [SerializeField] private RealitySenseWorker realitySense;

        public bool HandTrackingAvailable => handTracker != null && handTracker.IsReady;
        public bool RealitySenseAvailable => realitySense != null && realitySense.IsRunning;

        private void Update()
        {
            // No overlay, camera preview, or expensive work is performed here.
        }
    }
}
