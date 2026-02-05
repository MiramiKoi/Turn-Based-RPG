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
            if (_model.IsExecutingRoute)
            {
                return;
            }

            var currentCell = _world.GridInteractionModel.CurrentCell;
            if (currentCell != null && _model.CanAttack(currentCell.Position))
            {
                MakeStep();
                _model.Attack(currentCell.Position);
                
                _world.TurnBaseModel.PlayerStep();
            }
            else if (_model.HasPath())
            {
                MakeStep();
                _model.ExecuteNextStep();
                
                _world.TurnBaseModel.PlayerStep();
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
                _world.GridInteractionModel.IsActive.Value = true;
                _world.CameraControlModel.IsActive.Value = true;
            }
        }
    }
}