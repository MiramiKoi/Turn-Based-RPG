using Unity.Cinemachine;
using UnityEngine;

namespace Runtime.CameraControl
{
    public class CameraControlView : MonoBehaviour
    {
        public CinemachineCamera Camera => _camera;
        public CinemachinePositionComposer PositionComposer => _positionComposer;
        
        [SerializeField] private CinemachineCamera _camera;

        [SerializeField] private CinemachinePositionComposer _positionComposer;
    }
}