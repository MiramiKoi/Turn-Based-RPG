using System.Linq;
using Runtime.Common;
using Runtime.Landscape.Grid.Indication;
using Runtime.Player.Movement;
using Runtime.Units;
using UnityEngine;

namespace Runtime.Player
{
    public class PlayerPresenter : IPresenter
    {
        private readonly UnitModel _model;
        private readonly World _world;
        private readonly GridPathfinder _pathfinder;
        private readonly MovementQueue _movementQueue;

        public PlayerPresenter(UnitModel model, World world)
        {
            _model = model;
            _world = world;
            _pathfinder = new GridPathfinder();
            _movementQueue = new MovementQueue();
        }

        public void Enable()
        {
            _world.GridInteractionModel.OnCurrentCellChanged += HandlePointerMove;
        } 

        public void Disable()
        {
            _world.GridInteractionModel.OnCurrentCellChanged -= HandlePointerMove;
        }

        private void HandlePointerMove()
        {
            if (_world.GridInteractionModel.CurrentCell == null)
                return;

            var start = _model.Position.Value;
            var target = _world.GridInteractionModel.CurrentCell.Position;

            if (_movementQueue.HasSteps)
            {
                foreach (var position in _movementQueue.Steps)
                    _world.GridModel.Cells[position.x, position.y].SetIndication(IndicationType.Null);
            }

            var path = _pathfinder.FindPath(
                _world.GridModel,
                start,
                target);

            _movementQueue.SetPath(path);
            
            foreach (var position in path.Where(position => _model.Position.Value != position))
                _world.GridModel.Cells[position.x, position.y].SetIndication(IndicationType.RoutePoint);
        }

        private void MoveCharacter()
        {
            if (!_movementQueue.HasSteps)
                return;

            var nextCell = _movementQueue.Dequeue();

            if (!_world.GridModel.CanPlace(nextCell))
            {
                _movementQueue.Clear();
                return;
            }

            RotateCharacter(nextCell);

            _world.GridModel.ReleaseCell(_model.Position.Value);
            _world.GridModel.TryPlace(_model, nextCell);
            _model.MoveTo(nextCell);
        }
        
        private void RotateCharacter(Vector2Int next)
        {
            var current = _model.Position.Value;
            if (next.x == current.x)
                return;

            _model.Rotate(
                next.x < current.x
                    ? UnitDirection.Left
                    : UnitDirection.Right);
        }
    }
}