using Runtime.Common;
using Runtime.Core;
using Runtime.Landscape.Grid.Indication;
using UnityEngine.InputSystem;

namespace Runtime.Player
{
    public class PlayerInteractionPresenter : IPresenter
    {
        private readonly PlayerModel _model;
        private readonly World _world;
        private readonly PlayerCommandExecutor _executor;

        public PlayerInteractionPresenter(PlayerModel model, World world)
        {
            _model = model;
            _world = world;

            _executor = new PlayerCommandExecutor(model, world);
        }

        public void Enable()
        {
            _world.PlayerControls.Gameplay.Attack.performed += OnAttack;
            _world.PlayerControls.Gameplay.SkipTurn.performed += HandleSkipTurn;
            _world.TurnBaseModel.OnWorldStepFinished += OnTurnFinished;
        }

        public void Disable()
        {
            _world.PlayerControls.Gameplay.Attack.performed -= OnAttack;
            _world.PlayerControls.Gameplay.SkipTurn.performed -= HandleSkipTurn;
            _world.TurnBaseModel.OnWorldStepFinished -= OnTurnFinished;
        }

        private bool ShouldBlockInput()
        {
            return _model.IsDead ||
                   _model.IsExecutingRoute ||
                   _world.UIBlockerModel.IsPointerOverUI.Value ||
                   _world.GridInteractionModel.CurrentCell == null;
        }

        private void StepStart()
        {
            _world.GridInteractionModel.IsActive.Value = false;
            _world.CameraControlModel.IsActive.Value = false;
            _world.CameraControlModel.ResetCameraPosition();
        }

        private void ExecuteRouteStep()
        {
            if (!_model.IsExecutingRoute)
                StartRoute();

            var executed = _executor.ExecuteNext();

            if (!executed)
                StopRoute();
        }

        private void StartRoute()
        {
            _model.IsExecutingRoute = true;
            _world.GridModel.SetIndication(
                _model.MovementQueueModel.Steps,
                IndicationType.RoutePoint);
        }

        private void StopRoute()
        {
            _model.IsExecutingRoute = false;

            _world.GridModel.SetIndication(
                _model.MovementQueueModel.Steps,
                IndicationType.Null);

            _model.MovementQueueModel.Clear();
        }

        private void FinishStep()
        {
            if (_model.IsExecutingRoute)
            {
                _world.TurnBaseModel.PlayerStep();
            }
            else
            {
                _world.GridInteractionModel.IsActive.Value = true;
                _world.CameraControlModel.IsActive.Value = true;
            }
        }

        private void HandleSkipTurn(InputAction.CallbackContext obj)
        {
            StepStart();
            _world.TurnBaseModel.PlayerStep();
        }
        
        private void OnTurnFinished()
        {
            if (_model.IsDead || _model.Mode != PlayerMode.Adventure)
            {
                StopRoute();
            }

            if (_model.IsExecutingRoute)
            {
                ExecuteRouteStep();
            }

            FinishStep();
        }
        
        private void OnAttack(InputAction.CallbackContext _)
        {
            if (ShouldBlockInput())
                return;

            StepStart();
            ExecuteRouteStep();
            FinishStep();
        }
    }
}