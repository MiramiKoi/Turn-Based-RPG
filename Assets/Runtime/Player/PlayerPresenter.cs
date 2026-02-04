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
            if (!_model.IsExecutingRoute && _model.HasPath())
            {
                _world.GridInteractionModel.IsActive.Value = false;
                
                _world.CameraControlModel.IsActive.Value = false;
                _world.CameraControlModel.ResetCameraPosition();
                
                _model.ExecuteNextStep();
                _world.TurnBaseModel.PlayerStep();
            }
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
            _model.ExecuteNextStep();
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