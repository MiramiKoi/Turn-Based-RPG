using Runtime.Common;
using Runtime.Core;
using Runtime.Landscape.Grid.Indication;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.CameraControl
{
    public class CameraControlPresenter : IPresenter
    {
        private readonly CameraControlModel _model;
        private readonly CameraControlView _view;
        private readonly World _world;
        private readonly CameraControlSystem _system;
        private readonly CompositeDisposable _disposables = new();

        private Vector3 _damping;

        public CameraControlPresenter(CameraControlModel model, CameraControlView view, World world)
        {
            _model = model;
            _view = view;
            _world = world;

            _system = new CameraControlSystem(_model, _view, _world);
        } 
        
        public void Enable()
        {
            _model.Target.Subscribe(HandleTargetChanged).AddTo(_disposables);
            
            _world.PlayerControls.Gameplay.CameraControl.started += HandleCameraControllingStarted;
            _world.PlayerControls.Gameplay.CameraControl.canceled += HandleCameraControllingCanceled;

            _world.GameSystems.Add(_system);
        }

        public void Disable()
        {
            _disposables.Dispose();
            
            _world.PlayerControls.Gameplay.CameraControl.started -= HandleCameraControllingStarted;
            _world.PlayerControls.Gameplay.CameraControl.canceled -= HandleCameraControllingCanceled;

            _world.GameSystems.Remove(_system);
        }

        private void HandleTargetChanged(Transform target)
        {
            _view.Camera.Follow = target;
        }
        
        private void HandleCameraControllingStarted(InputAction.CallbackContext obj)
        {
            _model.IsManualControl = true;
            _damping = _view.PositionComposer.Damping;
            _view.PositionComposer.Damping = Vector3.zero;
            _world.GridInteractionModel.IsActive.Value = false;
            foreach (var cellModel in _world.GridModel.Cells)
            {
                cellModel.SetIndication(IndicationType.Null);
            }
            _model.LastPointerPosition = _world.PlayerControls.Gameplay.PointerPosition.ReadValue<Vector2>();
        }
        
        private void HandleCameraControllingCanceled(InputAction.CallbackContext obj)
        {
            _model.IsManualControl = false;
            _world.GridInteractionModel.IsActive.Value = true;
            
            _view.PositionComposer.Damping = _damping;
        }
    }
}