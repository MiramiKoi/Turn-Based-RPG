using System;
using UniRx;
using UnityEngine;

namespace Runtime.CameraControl
{
    public class CameraControlModel
    {
        public event Action OnResetCameraPosition;

        public BoolReactiveProperty IsActive { get; set; } = new(true);
        public ReactiveProperty<Transform> Target { get; private set; } = new();
        public bool IsManualControl { get; set; }
        public bool IsEdgeScrollEnabled => true;

        public Vector2 LastPointerPosition;
        
        public void ResetCameraPosition()
        {
            OnResetCameraPosition?.Invoke();
        }
    }
}