using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;

namespace HandShift.Quest
{
    /// Reads Unity XR Hands supplied by the active Quest OpenXR provider.
    /// This data is consumed inside HANDSHIFT only.
    public sealed class HandShiftHandTracker : MonoBehaviour
    {
        public struct Snapshot
        {
            public bool IsTracked;
            public Pose Wrist, Palm, IndexTip, ThumbTip;
            public double Timestamp;
        }

        public Snapshot Left { get; private set; }
        public Snapshot Right { get; private set; }
        public bool IsReady => subsystem != null && subsystem.running;

        private XRHandSubsystem subsystem;
        private readonly List<XRHandSubsystem> candidates = new List<XRHandSubsystem>();

        private void Update()
        {
            if (subsystem == null || !subsystem.running)
            {
                SubsystemManager.GetSubsystems(candidates);
                subsystem = null;
                foreach (var candidate in candidates)
                    if (candidate != null && candidate.running) { subsystem = candidate; break; }
            }
            if (subsystem == null || !subsystem.running) return;
            Left = Read(subsystem.leftHand);
            Right = Read(subsystem.rightHand);
        }

        private static Snapshot Read(XRHand hand)
        {
            if (!hand.isTracked) return default;
            return new Snapshot {
                IsTracked = true,
                Wrist = GetPose(hand, XRHandJointID.Wrist),
                Palm = GetPose(hand, XRHandJointID.Palm),
                IndexTip = GetPose(hand, XRHandJointID.IndexTip),
                ThumbTip = GetPose(hand, XRHandJointID.ThumbTip),
                Timestamp = Time.realtimeSinceStartupAsDouble
            };
        }

        private static Pose GetPose(XRHand hand, XRHandJointID id)
        {
            var joint = hand.GetJoint(id);
            return joint.TryGetPose(out Pose pose) ? pose : default;
        }
    }
}
