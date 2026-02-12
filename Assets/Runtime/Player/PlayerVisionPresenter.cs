using System;
using Runtime.Common;
using Runtime.Common.Vision;
using Runtime.Landscape.Grid;
using Runtime.Units;
using Runtime.Units.Collection;
using UniRx;
using UnityEngine;

namespace Runtime.Player
{
    public class PlayerVisionPresenter : IPresenter
    {
        private readonly GridModel _grid;
        private readonly UnitModelCollection _units;
        private readonly UnitModel _model;

        private IDisposable _positionSubscription;


        public PlayerVisionPresenter(GridModel grid, UnitModelCollection units, UnitModel model)
        {
            _grid = grid;
            _units = units;
            _model = model;
        }

        public void Enable()
        {
            _positionSubscription = _model.State.Position.Subscribe(OnPositionChange);

            OnPositionChange(_model.State.Position.Value);
        }

        public void Disable()
        {
            _positionSubscription?.Dispose();
        }

        private void OnPositionChange(Vector2Int position)
        {
            foreach (var unit in _units.Models.Values)
            {
                if (unit == _model)
                {
                    continue;
                }

                var visibilityRadius = Mathf.RoundToInt(_model.Stats["visibility_radius"].Value);

                unit.State.Visible.Value =
                    VisionPathFinder.Trace(_grid, position, unit.State.Position.Value, visibilityRadius);
            }
        }
    }
}