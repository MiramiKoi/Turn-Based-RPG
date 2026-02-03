using Runtime.Core;
using Runtime.Descriptions.CameraControl;
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
        private readonly Vector2 _screenSize;
        private readonly CameraControlDescription _description;

        public CameraControlSystem(CameraControlModel model, CameraControlView view, World world)
        {
            _model = model;
            _view = view;
            _world = world;
            _description = _world.WorldDescription.CameraControlDescription;

            _screenSize = new Vector2(Screen.width, Screen.height);
        }

        public void Update(float deltaTime)
        {
            if (_model.IsActive.Value)
            {
                var pointerPosition = GetPointerPosition();

                if (!TryApplyDrag(pointerPosition))
                {
                    TryApplyEdgeScroll(pointerPosition, deltaTime);
                }
            }
        }
        
        private bool TryApplyDrag(Vector2 pointerPosition)
        {
            if (_model.IsManualControl)
            {
                var delta = pointerPosition - _model.LastPointerPosition;
                _model.LastPointerPosition = pointerPosition;

                var drag = CalculateDrag(delta);
                ApplyMove(drag);

                return true;
            }

            return false;
        }
        
        private Vector3 CalculateDrag(Vector2 pointerDelta)
        {
            var unitsPerPixel = _view.Camera.Lens.OrthographicSize / _screenSize.y;

            var drag = new Vector3(-pointerDelta.x, -pointerDelta.y, 0f);
            drag *= unitsPerPixel * _description.DragSensitivity;

            return drag;
        }
        
        private void TryApplyEdgeScroll(Vector2 pointerPosition, float deltaTime)
        {
            if (_model.IsEdgeScrollEnabled)
            {
                var direction = GetEdgeDirection(pointerPosition);
                if (direction != Vector2.zero)
                {
                    var move = CalculateEdgeMove(direction, deltaTime);
                    ApplyMove(move);
                }
            }
        }

        private Vector2 GetEdgeDirection(Vector2 pointerPosition)
        {
            var direction = Vector2.zero;

            if (pointerPosition.x <= _description.EdgeSize)
                direction.x = -1;
            else if (pointerPosition.x >= _screenSize.x - _description.EdgeSize)
                direction.x = 1;

            if (pointerPosition.y <= _description.EdgeSize)
                direction.y = -1;
            else if (pointerPosition.y >= _screenSize.y - _description.EdgeSize)
                direction.y = 1;

            return direction;
        }

        private Vector3 CalculateEdgeMove(Vector2 direction, float deltaTime)
        {
            return new Vector3(direction.x, direction.y, 0f) * (_description.EdgeSensitivity * deltaTime);
        }

        private Vector2 GetPointerPosition()
        {
            return _world.PlayerControls.Gameplay.PointerPosition.ReadValue<Vector2>();
        }

        private void ApplyMove(Vector3 move)
        {
            _view.PositionComposer.TargetOffset += move;
        }
    }
}