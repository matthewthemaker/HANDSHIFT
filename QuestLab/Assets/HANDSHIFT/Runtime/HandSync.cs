using UnityEngine;

namespace HandShift.Quest
{
    public sealed class HandSync
    {
        private Quaternion rotationOffset = Quaternion.identity;
        private Vector3 positionOffset;
        public bool IsCalibrated { get; private set; }

        public void Calibrate(Pose physicalPalm, Pose virtualPalm)
        {
            rotationOffset = virtualPalm.rotation * Quaternion.Inverse(physicalPalm.rotation);
            positionOffset = virtualPalm.position - rotationOffset * physicalPalm.position;
            IsCalibrated = true;
        }

        public Pose Map(Pose physicalPose)
        {
            if (!IsCalibrated) return physicalPose;
            return new Pose(positionOffset + rotationOffset * physicalPose.position,
                            rotationOffset * physicalPose.rotation);
        }

        public void Reset()
        {
            IsCalibrated = false;
            rotationOffset = Quaternion.identity;
            positionOffset = Vector3.zero;
        }
    }
}
