using UniRx;
using UnityEngine;

namespace Runtime.CameraControl
{
    public class CameraControlModel
    {
        public ReactiveProperty<Transform> Target { get; private set; } = new();
    }
}