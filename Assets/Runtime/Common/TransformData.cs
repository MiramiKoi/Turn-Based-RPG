using UnityEngine;

namespace Runtime.Common
{
    public struct TransformData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;

        public TransformData(Transform t)
        {
            Position = t.localPosition;
            Rotation = t.localRotation;
            Scale = t.localScale;
        }

        public void Apply(Transform t)
        {
            t.localPosition = Position;
            t.localRotation = Rotation;
            t.localScale = Scale;
        }
    }
}