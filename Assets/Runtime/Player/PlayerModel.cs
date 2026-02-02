using System.Linq;
using Runtime.Common.Movement;
using Runtime.Landscape.Grid;
using Runtime.Landscape.Grid.Indication;
using Runtime.Units;
using UnityEngine;

namespace Runtime.Player
{
    public class PlayerModel
    {
        private readonly UnitModel _model;
        private readonly GridModel _grid;

        private readonly MovementQueueModel _movementQueueModel = new();

        public bool IsExecutingRoute { get; private set; }

        public PlayerModel(UnitModel model, GridModel world)
        {
            _model = model;
            _grid = world;
        }

        public void FindPath(Vector2Int target)
        {
            var start = _model.Position.Value;

            if (_movementQueueModel.HasSteps)
            {
                foreach (var position in _movementQueueModel.Steps)
                    _grid.Cells[position.x, position.y].SetIndication(IndicationType.Null);
            }

            var path = GridPathfinder.FindPath(_grid, start, target);

            _movementQueueModel.SetPath(path);

            foreach (var position in path.Where(position => _model.Position.Value != position))
            {
                _grid.Cells[position.x, position.y].SetIndication(IndicationType.RoutePoint);
            }
        }

        public bool HasPath() => _movementQueueModel.HasSteps;
        
        public void ExecuteNextStep()
        {
            if (!IsExecutingRoute)
            {
                IsExecutingRoute = true;
            }
            
            if (_movementQueueModel.TryDequeue(out var nextCell) && _grid.CanPlace(nextCell))
            {
                _grid.ReleaseCell(_model.Position.Value);
                _grid.TryPlace(_model, nextCell);
                _model.MoveTo(nextCell);
            }
            else
            {
                StopRoute();
            }
        }

        private void StopRoute()
        {
            IsExecutingRoute = false;
            ClearRouteIndication();
            _movementQueueModel.Clear();
        }

        private void ClearRouteIndication()
        {
            foreach (var position in _movementQueueModel.Steps)
            {
                _grid.Cells[position.x, position.y].SetIndication(IndicationType.Null);
            }
        }
    }
}