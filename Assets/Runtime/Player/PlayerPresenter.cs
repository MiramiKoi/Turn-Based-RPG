using Runtime.Common;
using Runtime.Player.Movement;
using Runtime.Units;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Player
{
    public class PlayerPresenter : IPresenter
    {
        private readonly UnitModel _character;
        private readonly World _world;
        private readonly GridPathfinder _pathfinder;
        private readonly MovementQueue _movementQueue;

        public PlayerPresenter(UnitModel character, World world)
        {
            _character = character;
            _world = world;
            _pathfinder = new GridPathfinder();
            _movementQueue = new MovementQueue();
        }

        public void Enable()
        {
            _world.PlayerControls.Gameplay.Attack.performed += HandleAttack;
        }

        public void Disable()
        {
            _world.PlayerControls.Gameplay.Attack.performed -= HandleAttack;
        }

        private void HandleAttack(InputAction.CallbackContext obj)
        {
            if (_world.GridInteractionModel.CurrentCell == null)
                return;

            var start = _character.Position.Value;
            var target = _world.GridInteractionModel.CurrentCell.Position;

            var path = _pathfinder.FindPath(
                _world.GridModel,
                start,
                target);

            _movementQueue.SetPath(path);
            
            if (!_movementQueue.HasSteps)
                return;

            var nextCell = _movementQueue.Dequeue();

            if (!_world.GridModel.CanPlace(nextCell))
            {
                _movementQueue.Clear();
                return;
            }

            RotateCharacter(nextCell);

            _world.GridModel.ReleaseCell(_character.Position.Value);
            _world.GridModel.TryPlace(_character, nextCell);
            _character.MoveTo(nextCell);
        }
        
        private void RotateCharacter(Vector2Int next)
        {
            var current = _character.Position.Value;
            if (next.x == current.x)
                return;

            _character.Rotate(
                next.x < current.x
                    ? UnitDirection.Left
                    : UnitDirection.Right);
        }
    }
}