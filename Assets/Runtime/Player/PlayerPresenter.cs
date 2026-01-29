using System;
using Runtime.Common;
using Runtime.Units;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Player
{
    public class PlayerPresenter : IPresenter
    {
        private readonly UnitModel _character;
        private readonly World _world;

        public PlayerPresenter(UnitModel character, World world)
        {
            _character = character;
            _world = world;
        }

        public void Enable()
        {
            _world.PlayerControls.Gameplay.Attack.performed += OnCellSelected;
        }

        public void Disable()
        {
            _world.PlayerControls.Gameplay.Attack.performed -= OnCellSelected;
        }

        private void OnCellSelected(InputAction.CallbackContext context)
        {
            if (_world.GridInteractionModel.CurrentCell != null)
            {
                var selectedCell = _world.GridInteractionModel.CurrentCell;

                var newCellPositionX = selectedCell.Position.x - _character.Position.Value.x;
                var newCellPositionY = selectedCell.Position.y - _character.Position.Value.y;

                newCellPositionX = Math.Clamp(newCellPositionX, -1, 1);
                newCellPositionY = Math.Clamp(newCellPositionY, -1, 1);

                newCellPositionX += _character.Position.Value.x;
                newCellPositionY += _character.Position.Value.y;

                var newCellPosition = new Vector2Int(newCellPositionX, newCellPositionY);

                if (_world.GridModel.CanPlace(newCellPosition))
                {
                    if (newCellPosition.x != _character.Position.Value.x)
                    {
                        var newDirection = newCellPosition.x < _character.Position.Value.x
                            ? UnitDirection.Left
                            : UnitDirection.Right;
                        _character.Rotate(newDirection);
                    }

                    _world.GridModel.ReleaseCell(_character.Position.Value);
                    _world.GridModel.TryPlace(_character, newCellPosition);

                    _character.MoveTo(newCellPosition);
                }
            }
        }
    }
}