using Unity.Cinemachine;
using UnityEngine;

namespace Runtime.CameraControl
{
    public class CameraControlView : MonoBehaviour
    {
        public CinemachineCamera Camera => _camera;
        
        [SerializeField] private CinemachineCamera _camera;
    }
}