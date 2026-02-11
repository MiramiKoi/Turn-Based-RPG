using System.Collections.Generic;
using System.Linq;
using Runtime.Common.Movement;
using Runtime.Core;
using Runtime.Landscape.Grid.Indication;
using Runtime.Units;
using Runtime.ViewDescriptions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Player
{
    public class PlayerPresenter : UnitPresenter
    {
        private readonly PlayerModel _model;
        private readonly World _world;

        private PlayerVisionPresenter _visionPresenter;

        public PlayerPresenter(PlayerModel model, UnitView view, World world, WorldViewDescriptions worldViewDescriptions) 
            : base(model, view, world, worldViewDescriptions)
        {
            _model = model;
            _world = world;
        }

        public override void Enable()
        {
            base.Enable();
            
            _world.GridInteractionModel.OnCurrentCellChanged += HandleInteractionCellChanged;
            _world.PlayerControls.Gameplay.Attack.performed += HandleAttackPerformed;
            _world.TurnBaseModel.OnWorldStepFinished += HandleTurnFinished;
            _visionPresenter = new PlayerVisionPresenter(_world.GridModel, _world.UnitCollection, _model);
            _visionPresenter.Enable();
        }

        public override void Disable()
        {
            base.Disable();
            
            _world.GridInteractionModel.OnCurrentCellChanged -= HandleInteractionCellChanged;
            _world.PlayerControls.Gameplay.Attack.performed -= HandleAttackPerformed;
            _world.TurnBaseModel.OnWorldStepFinished -= HandleTurnFinished;

            _visionPresenter.Disable();
            _visionPresenter = null;
        }

        private void HandleInteractionCellChanged()
        {
            if (!_model.IsDead && !_model.IsExecutingRoute && _world.GridInteractionModel.CurrentCell != null)
            {
                ClearRouteIndication();
                _model.MovementQueueModel.Clear();

                var start = _model.Position.Value;
                if (GridPathfinder.FindPath(_world.GridModel, start, _world.GridInteractionModel.CurrentCell.Position,
                        out var path))
                {
                    _model.MovementQueueModel.SetPath(path);
                    DrawRoute(path.Where(position => _model.Position.Value != position));
                }
            }
        }
        
        private void HandleAttackPerformed(InputAction.CallbackContext obj)
        {
            if (_model.IsDead || _model.IsExecutingRoute || _world.UIBlocker.IsPointerOverUI || _world.GridInteractionModel.CurrentCell == null)
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
            
            if (_model.CanMove() && _world.GridModel.TryPlace(_model, nextCell))
            {
                _world.GridModel.ReleaseCell(_model.Position.Value);
                _model.MoveTo(nextCell);
            }
            else if (_world.GridModel.GetCell(nextCell).Unit is UnitModel unit && unit != _model)
            {
                if (unit.IsDead) 
                {
                    _world.LootModel.RequestLoot(unit);
                    StopRoute();
                }
                else if (unit.Description.Fraction != _model.Description.Fraction && _model.CanAttack(nextCell))
                {
                    var enemy = (UnitModel)_world.GridModel.GetCell(nextCell).Unit;
                    var damage = _model.GetDamage();
                    enemy.TakeDamage(damage);
                }
            }
            else
            {
                StopRoute();
            }
        }

        private void StartRoute()
        {
            _model.IsExecutingRoute = true;
            DrawRoute(_model.MovementQueueModel.Steps);
        }
        
        private void StopRoute()
        {
            _model.IsExecutingRoute = false;
            ClearRouteIndication();
            _model.MovementQueueModel.Clear();
        }
        
        private void ClearRouteIndication()
        {
            foreach (var position in _model.MovementQueueModel.Steps)
            {
                _world.GridModel.Cells[position.x, position.y].SetIndication(IndicationType.Null);
            }
        }

        private void DrawRoute(IEnumerable<Vector2Int> path)
        {
            foreach (var position in path)
            {
                _world.GridModel.Cells[position.x, position.y].SetIndication(IndicationType.RoutePoint);
            }
        }
    }
}