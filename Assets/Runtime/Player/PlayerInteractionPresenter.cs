using Runtime.Common;
using Runtime.Core;
using Runtime.Landscape.Grid.Indication;
using Runtime.Units;
using Runtime.Units.Components;
using UnityEngine.InputSystem;

namespace Runtime.Player
{
    public class PlayerInteractionPresenter : IPresenter
    {
        private readonly PlayerModel _model;
        private readonly World _world;

        public PlayerInteractionPresenter(PlayerModel model, World world)
        {
            _model = model;
            _world = world;
        }

        public void Enable()
        {
            _world.PlayerControls.Gameplay.Attack.performed += HandleAttackPerformed;
            _world.TurnBaseModel.OnWorldStepFinished += HandleTurnFinished;
        }

        public void Disable()
        {
            _world.PlayerControls.Gameplay.Attack.performed -= HandleAttackPerformed;
            _world.TurnBaseModel.OnWorldStepFinished -= HandleTurnFinished;
        }

        private void HandleAttackPerformed(InputAction.CallbackContext obj)
        {
            if (_model.IsDead || _model.IsExecutingRoute || _world.UIBlocker.IsPointerOverUI ||
                _world.GridInteractionModel.CurrentCell == null)
            {
                return;
            }

            StepStart();
            ExecuteNextStep();
            StepEnd();
        }

        private void HandleTurnFinished()
        {
            if (_model.IsDead)
            {
                _model.IsExecutingRoute = false;
            }

            if (_model.IsExecutingRoute)
            {
                ExecuteNextStep();
            }

            StepEnd();
        }

        private void StepStart()
        {
            _world.GridInteractionModel.IsActive.Value = false;

            _world.CameraControlModel.IsActive.Value = false;
            _world.CameraControlModel.ResetCameraPosition();
        }

        private void StepEnd()
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

        private void ExecuteNextStep()
        {
            if (!_model.IsExecutingRoute)
            {
                StartRoute();
            }

            var hasNextCell = _model.MovementQueueModel.TryDequeue(out var nextCell);
            if (!hasNextCell)
            {
                StopRoute();
                return;
            }

            if (_model.ActionBlocker.CanExecute(UnitActionType.Move) && _world.GridModel.TryPlace(_model, nextCell))
            {
                _world.GridModel.ReleaseCell(_model.State.Position.Value);
                _model.Movement.MoveTo(nextCell);
            }
            else
            {
                if (_world.GridModel.GetCell(nextCell).Unit is UnitModel unit && unit != _model)
                {
                    if (unit.IsDead || unit.Description.Fraction == "trader")
                    {
                        _world.LootModel.RequestLoot(unit);
                        StopRoute();
                    }
                    else if (unit.Description.Fraction != _model.Description.Fraction &&
                             _model.Combat.CanAttack(nextCell))
                    {
                        var enemy = (UnitModel)_world.GridModel.GetCell(nextCell).Unit;
                        var damage = _model.Combat.GetDamage();
                        enemy.Combat.TakeDamage(damage);
                    }
                }
                else
                {
                    StopRoute();
                    if (!_model.ActionBlocker.CanExecute(UnitActionType.Move))
                    {
                        _model.IsExecutingRoute = true;
                    }
                }
            }
        }

        private void StartRoute()
        {
            _model.IsExecutingRoute = true;
            _world.GridModel.SetIndication(_model.MovementQueueModel.Steps, IndicationType.RoutePoint);
        }

        private void StopRoute()
        {
            _model.IsExecutingRoute = false;
            _world.GridModel.SetIndication(_model.MovementQueueModel.Steps, IndicationType.Null);
            _model.MovementQueueModel.Clear();
        }
    }
}