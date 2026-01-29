using Runtime.Common;
using Runtime.Landscape.Grid.Indication;

namespace Runtime.Landscape.Grid.Interaction
{
    public class GridInteractionPresenter : IPresenter
    {
        private readonly GridInteractionModel _model;
        private readonly World _world;
        
        private readonly GridInteractionSystem _system;

        public GridInteractionPresenter(GridInteractionModel model, GridView view, World world)
        {
            _model = model;
            _world = world;
            _system = new GridInteractionSystem(model, view, world);                
        }

        public void Enable()
        {
            _world.GameSystems.Add(_system);
        }

        public void Disable()
        {
            Clear();
            _world.GameSystems.Remove(_system);
        }

        private void Clear()
        {
            if (_model.CurrentCell != null)
                _world.GridModel.Cells[_model.CurrentCell.Position.x, _model.CurrentCell.Position.y].SetIndication(IndicationType.Null);
            _model.SetCell(null);
        }
    }
}