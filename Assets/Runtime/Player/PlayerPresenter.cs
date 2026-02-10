using Runtime.Common;
using Runtime.Core;
using UnityEngine.InputSystem;

namespace Runtime.Player
{
    public class PlayerPresenter : IPresenter
    {
        private readonly PlayerModel _model;
        private readonly World _world;

        public PlayerPresenter(PlayerModel model, World world)
        {
            _model = model;
            _world = world;
        }

        public void Enable()
        {
            _world.GridInteractionModel.OnCurrentCellChanged += HandleInteractionCellChanged;
            _world.PlayerControls.Gameplay.Attack.performed += HandleAttackPerformed;
            _world.TurnBaseModel.OnWorldStepFinished += HandleTurnFinished;
        }

        public void Disable()
        {
            _world.GridInteractionModel.OnCurrentCellChanged -= HandleInteractionCellChanged;
            _world.PlayerControls.Gameplay.Attack.performed -= HandleAttackPerformed;
            _world.TurnBaseModel.OnWorldStepFinished -= HandleTurnFinished;
        }

        private void HandleAttackPerformed(InputAction.CallbackContext obj)
        {
            if (_model.IsExecutingRoute || _world.UIBlocker.IsPointerOverUI)
            {
                return;
            }
            
            var currentCell = _world.GridInteractionModel.CurrentCell;

            if (currentCell == null)
            {
                return;
            }
            
            var isDeadUnitCell = currentCell.Unit is { IsDead: true };
            
            if (_model.CanAttack(currentCell.Position))
            {
                MakeStep();
                _model.Attack(currentCell.Position);

                _world.TurnBaseModel.PlayerStep();
            }
            else if (_model.HasPath())
            {
                MakeStep();
                _model.ExecuteNextStep();

                if (isDeadUnitCell)
                {
                    _world.LootModel.PendingLootCell = currentCell;
                }

                _world.TurnBaseModel.PlayerStep();
            }
            else if (isDeadUnitCell)
            {
                _world.LootModel.RequestLoot(currentCell.Unit);
            }
        }

        private void MakeStep()
        {
            _world.GridInteractionModel.IsActive.Value = false;

            _world.CameraControlModel.IsActive.Value = false;
            _world.CameraControlModel.ResetCameraPosition();
        }

        private void HandleInteractionCellChanged()
        {
            if (!_model.IsExecutingRoute && _world.GridInteractionModel.CurrentCell != null)
            {
                _model.FindPath(_world.GridInteractionModel.CurrentCell.Position);
            }
        }

        private void HandleTurnFinished()
        {
            if (_model.IsExecutingRoute)
            {
                _model.ExecuteNextStep();
            }

            if (_model.IsExecutingRoute)
            {
                _world.TurnBaseModel.PlayerStep();
            }
            else
            {
                if (_world.LootModel.PendingLootCell?.Unit != null)
                {
                    _world.LootModel.RequestLoot(_world.LootModel.PendingLootCell.Unit);
                    _world.LootModel.PendingLootCell = null;
                }
                
                _world.GridInteractionModel.IsActive.Value = true;
                _world.CameraControlModel.IsActive.Value = true;
            }
        }
    }
}