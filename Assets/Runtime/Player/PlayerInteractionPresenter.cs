using Runtime.Common;
using Runtime.Core;
using Runtime.Landscape.Grid.Indication;
using UnityEngine;
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
            _world.PlayerControls.Gameplay.Attack.performed += HandleAttack;
            _world.PlayerControls.Gameplay.SkipTurn.performed += HandleSkipTurn;
            _world.PlayerControls.Gameplay.ToggleAttackMode.performed += HandleToggleAttackMode;
            _world.TurnBaseModel.OnWorldStepFinished += HandleTurnFinished;
        }

        public void Disable()
        {
            _world.PlayerControls.Gameplay.Attack.performed -= HandleAttack;
            _world.PlayerControls.Gameplay.SkipTurn.performed -= HandleSkipTurn;
            _world.PlayerControls.Gameplay.ToggleAttackMode.performed -= HandleToggleAttackMode;
            _world.TurnBaseModel.OnWorldStepFinished -= HandleTurnFinished;
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
            if (_model.Mode != PlayerMode.Attack)
            {
                _world.GridModel.SetIndication(
                    _model.MovementQueueModel.Steps,
                    IndicationType.RoutePoint);
            }
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

        private void ChangePlayerMode()
        {
            if (_model.Mode == PlayerMode.Attack)
                return;
            
            _model.Mode = _world.TurnBaseModel.BattleModel.IsInBattle() ? PlayerMode.Battle : PlayerMode.Adventure;
        }
        
        private void HandleSkipTurn(InputAction.CallbackContext obj)
        {
            if (ShouldBlockInput())
                return;
            
            StepStart();
            _world.TurnBaseModel.PlayerStep();
        }
        
        private void HandleTurnFinished()
        {
            if (_model.IsDead || _model.Mode != PlayerMode.Adventure)
            {
                StopRoute();
            }

            ChangePlayerMode();
            
            if (_model.IsExecutingRoute)
            {
                ExecuteRouteStep();
            }

            FinishStep();
        }
        
        private void HandleAttack(InputAction.CallbackContext _)
        {
            if (ShouldBlockInput())
                return;

            StepStart();
            ExecuteRouteStep();
            FinishStep();
        }

        private void HandleToggleAttackMode(InputAction.CallbackContext _)
        {
            if (ShouldBlockInput())
                return;

            if (_model.Mode != PlayerMode.Attack)
            {
                _model.Mode = PlayerMode.Attack;
                _world.GridModel.SetIndication(
                    _model.MovementQueueModel.Steps,
                    IndicationType.Null);
            }
            else
            {
                _model.Mode = _world.TurnBaseModel.BattleModel.IsInBattle() ? PlayerMode.Battle : PlayerMode.Adventure;
                _world.GridModel.SetIndication(
                    _model.MovementQueueModel.Steps,
                    IndicationType.RoutePoint);
            }
        }
    }
}