using System.Linq;
using Runtime.Common.Movement;
using Runtime.Core;
using Runtime.Landscape.Grid.Indication;
using Runtime.Units;
using UnityEngine.InputSystem;

namespace Runtime.Player
{
    public class PlayerPresenter : UnitPresenter
    {
        private readonly UnitModel _model;
        private readonly World _world;
        private readonly MovementQueueModel _movementQueueModel;

        private bool _isExecutingRoute;

        public PlayerPresenter(UnitModel model, UnitView unitView, World world) : base(model, unitView)
        {
            _model = model;
            _world = world;
            _movementQueueModel = new MovementQueueModel();
        }

        public override void Enable()
        {
            base.Enable();
            _world.GridInteractionModel.OnCurrentCellChanged += HandleInteractionCellChanged;
            _world.PlayerControls.Gameplay.Attack.performed += HandleAttackPerformed;
            _world.TurnBaseModel.OnStepFinished += HandleTurnFinished;
        }

        public override void Disable()
        {
            base.Disable();
            _world.GridInteractionModel.OnCurrentCellChanged -= HandleInteractionCellChanged;
            _world.PlayerControls.Gameplay.Attack.performed -= HandleAttackPerformed;
            _world.TurnBaseModel.OnStepFinished -= HandleTurnFinished;
        }

        private async void ExecuteNextStep()
        {
            if (_isExecutingRoute)
            {
                if (_movementQueueModel.HasSteps)
                {
                    var nextCell = _movementQueueModel.Dequeue();

                    if (_world.GridModel.CanPlace(nextCell))
                    {
                        _world.GridModel.ReleaseCell(_model.Position.Value);
                        _world.GridModel.TryPlace(_model, nextCell);
                        _model.MoveTo(nextCell);
                        _world.GridInteractionModel.IsActive = false;

                        await _model.Awaiter;
                        _world.TurnBaseModel.Step();
                    }
                    else
                    {
                        StopRoute();
                    }
                }
                else
                {
                    StopRoute();
                }
            }
        }
        private void StopRoute()
        {
            _world.GridInteractionModel.IsActive = true;
            _isExecutingRoute = false;
            _movementQueueModel.Clear();
            ClearRouteIndication();
        }

        private void ClearRouteIndication()
        {
            foreach (var position in _movementQueueModel.Steps)
            {
                _world.GridModel.Cells[position.x, position.y].SetIndication(IndicationType.Null);
            }
        }

        private void HandleAttackPerformed(InputAction.CallbackContext obj)
        {
            if (_movementQueueModel.HasSteps && !_isExecutingRoute)
            {
                _isExecutingRoute = true;
                ExecuteNextStep();
            }
        }

        private void HandleTurnFinished()
        {
            if (_isExecutingRoute)
                ExecuteNextStep();
        }

        private void HandleInteractionCellChanged()
        {
            if (!_isExecutingRoute)
            {
                if (_world.GridInteractionModel.CurrentCell != null)
                {
                    var start = _model.Position.Value;
                    var target = _world.GridInteractionModel.CurrentCell.Position;

                    if (_movementQueueModel.HasSteps)
                    {
                        foreach (var position in _movementQueueModel.Steps)
                            _world.GridModel.Cells[position.x, position.y].SetIndication(IndicationType.Null);
                    }

                    if (GridPathfinder.FindPath(_world.GridModel, start, target, out var path))
                    {
                        _movementQueueModel.SetPath(path);

                        foreach (var position in path.Where(position => _model.Position.Value != position))
                        {
                            _world.GridModel.Cells[position.x, position.y].SetIndication(IndicationType.RoutePoint);
                        }
                    }
                }
            }
        }
    }
}