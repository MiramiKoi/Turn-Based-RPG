using System.Linq;
using Runtime.Common;
using Runtime.Common.Movement;
using Runtime.Core;
using Runtime.Landscape.Grid.Indication;

namespace Runtime.Player
{
    public class PlayerPathPresenter : IPresenter
    {
        private readonly PlayerModel _model;
        private readonly World _world;

        public PlayerPathPresenter(PlayerModel model, World world)
        {
            _model = model;
            _world = world;
        }

        public void Enable()
        {
            _world.GridInteractionModel.OnCurrentCellChanged += HandleInteractionCellChanged;
        }

        public void Disable()
        {
            _world.GridInteractionModel.OnCurrentCellChanged -= HandleInteractionCellChanged;
        }

        private void HandleInteractionCellChanged()
        {
            if (!_model.IsDead && !_model.IsExecutingRoute && _world.GridInteractionModel.CurrentCell != null)
            {
                _world.GridModel.SetIndication(_model.MovementQueueModel.Steps, IndicationType.Null);
                _model.MovementQueueModel.Clear();

                var start = _model.State.Position.Value;
                if (GridPathfinder.FindPath(_world.GridModel, start, _world.GridInteractionModel.CurrentCell.Position,
                        out var path))
                {
                    _model.MovementQueueModel.SetPath(path);
                    _world.GridModel.SetIndication(path.Where(position => _model.State.Position.Value != position),
                        IndicationType.RoutePoint);
                }
            }
        }
    }
}