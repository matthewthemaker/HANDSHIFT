using UnityEngine;

namespace HandShift.Quest
{
    /// Lab-only world-space markers for validating tracking. Not intended for the final game mod.
    public sealed class HandJointVisualizer : MonoBehaviour
    {
        [SerializeField] private HandShiftHandTracker tracker;
        [SerializeField] private float markerScale = 0.018f;
        private Transform[] left = new Transform[4];
        private Transform[] right = new Transform[4];

        private void Start()
        {
            for (int i = 0; i < 4; i++)
            {
                left[i] = CreateMarker("Left_" + i);
                right[i] = CreateMarker("Right_" + i);
            }
        }

        private Transform CreateMarker(string markerName)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = "HANDSHIFT_Lab_" + markerName;
            go.transform.localScale = Vector3.one * markerScale;
            var collider = go.GetComponent<Collider>();
            if (collider != null) Destroy(collider);
            return go.transform;
        }

        private void LateUpdate()
        {
            if (tracker == null) return;
            UpdateMarkers(left, tracker.Left);
            UpdateMarkers(right, tracker.Right);
        }

        private static void UpdateMarkers(Transform[] markers, HandShiftHandTracker.Snapshot hand)
        {
            if (!hand.IsTracked)
            {
                foreach (var marker in markers) marker.gameObject.SetActive(false);
                return;
            }
            Pose[] poses = { hand.Wrist, hand.Palm, hand.IndexTip, hand.ThumbTip };
            for (int i = 0; i < markers.Length; i++)
            {
                markers[i].gameObject.SetActive(true);
                markers[i].SetPositionAndRotation(poses[i].position, poses[i].rotation);
            }
        }
    }
}
