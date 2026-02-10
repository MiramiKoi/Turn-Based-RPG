using Runtime.Common;
using Runtime.Core;
using Runtime.Landscape.Grid.Indication;
using UniRx;

namespace Runtime.Landscape.Grid.Interaction
{
    public class GridInteractionPresenter : IPresenter
    {
        private readonly GridInteractionModel _model;
        private readonly World _world;

        private readonly GridInteractionSystem _system;
        private readonly CompositeDisposable _disposable = new();

        public GridInteractionPresenter(GridInteractionModel model, GridView view, World world)
        {
            _model = model;
            _world = world;
            _system = new GridInteractionSystem(model, view, world);
        }

        public void Enable()
        {
            _world.GameSystems.Add(_system);
            _model.IsActive.Subscribe(HandleIsActiveChanged).AddTo(_disposable);
        }

        public void Disable()
        {
            Clear();
            _world.GameSystems.Remove(_system);
            _disposable.Dispose();
        }

        private void Clear()
        {
            if (_model.CurrentCell == null)
            {
                return;
            }
            
            _world.GridModel.Cells[_model.CurrentCell.Position.x, _model.CurrentCell.Position.y].SetIndication(IndicationType.Null);
            _model.SetCell(null);
        }

        private void HandleIsActiveChanged(bool value)
        {
            if (!value)
            {
                Clear();
            }
        }
    }
}