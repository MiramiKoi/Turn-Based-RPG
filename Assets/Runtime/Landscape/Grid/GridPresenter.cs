using System.Collections.Generic;
using Runtime.Common;
using Runtime.Core;
using Runtime.Landscape.Grid.Cell;
using Runtime.ViewDescriptions;

namespace Runtime.Landscape.Grid
{
    public class GridPresenter : IPresenter
    {
        private readonly GridModel _model;
        private readonly GridView _view;
        private readonly World _world;
        private readonly WorldViewDescriptions _worldViewDescriptions;
        private readonly List<CellPresenter> _cells = new();


        public GridPresenter(GridModel model, GridView view, World world, WorldViewDescriptions worldViewDescriptions)
        {
            _model = model;
            _view = view;
            _world = world;
            _worldViewDescriptions = worldViewDescriptions;
        }

        public void Enable()
        {
            foreach (var cellModel in _model.Cells)
            {
                var cellView = new CellView(_view.Tilemap);

                var cellPresenter = new CellPresenter(cellModel, cellView, _world, _worldViewDescriptions);
                cellPresenter.Enable();

                _cells.Add(cellPresenter);
            }
        }

        public void Disable()
        {
            foreach (var cell in _cells)
            {
                cell.Disable();
            }

            _cells.Clear();
        }
    }
}