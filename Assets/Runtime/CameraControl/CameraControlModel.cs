using UniRx;
using UnityEngine;

namespace Runtime.CameraControl
{
    public class CameraControlModel
    {
        public ReactiveProperty<Transform> Target { get; private set; } = new();
        public bool IsManualControl { get; set; } = false;
        public Vector2 LastPointerPosition;
    }
}