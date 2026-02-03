using Runtime.Core;
using Runtime.GameSystems;
using UnityEngine;

namespace Runtime.CameraControl
{
    public class CameraControlSystem : IGameSystem
    {
        public string Id => "camera_control";

        private readonly CameraControlModel _model;
        private readonly CameraControlView _view;
        private readonly World _world;

        public CameraControlSystem(CameraControlModel model, CameraControlView view, World world)
        {
            _model = model;
            _view = view;
            _world = world;
        }

        public void Update(float deltaTime)
        {
            if (_model.IsManualControl)
            {
                var currentPointerPosition = _world.PlayerControls.Gameplay.PointerPosition.ReadValue<Vector2>();
                var delta = currentPointerPosition - _model.LastPointerPosition;
                _model.LastPointerPosition = currentPointerPosition;

                var drag = new Vector3(-delta.x, -delta.y, 0);
                drag *= _view.Camera.Lens.OrthographicSize / Screen.height * 2f;

                _view.PositionComposer.TargetOffset += drag;
            }
        }
    }
}