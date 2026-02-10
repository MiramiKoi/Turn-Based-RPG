using System.Collections.Generic;
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
            ClearRouteIndication();

            var start = _model.Position.Value;
            if (GridPathfinder.FindPath(_grid, start, target, out var path))
            {
                _movementQueueModel.SetPath(path);
                DrawRoute(path.Where(position => _model.Position.Value != position));
            }
        }

        public bool CanAttack(Vector2Int target)
        {
            return _grid.GetCell(target).Unit is UnitModel unit &&
                   unit != _model &&
                   !unit.IsDead &&
                   _model.CanAttack(target);
        }

        public void Attack(Vector2Int target)
        {
            if (!CanAttack(target))
            {
                return;
            }

            var enemy = (UnitModel)_grid.GetCell(target).Unit;
            var damage = _model.GetDamage();
            enemy.TakeDamage(damage);
        }

        public bool HasPath()
        {
            return _movementQueueModel.HasSteps;
        }

        public void ExecuteNextStep()
        {
            if (!IsExecutingRoute)
            {
                IsExecutingRoute = true;
                DrawRoute(_movementQueueModel.Steps);
            }

            if (_movementQueueModel.TryDequeue(out var nextCell) && _grid.CanPlace(nextCell) && _model.CanMove())
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

        private void DrawRoute(IEnumerable<Vector2Int> path)
        {
            foreach (var position in path)
            {
                _grid.Cells[position.x, position.y].SetIndication(IndicationType.RoutePoint);
            }
        }
    }
}