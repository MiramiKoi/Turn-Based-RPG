using Runtime.Common;
using Runtime.ViewDescriptions;

namespace Runtime.Landscape.Grid.Cell
{
    public class CellPresenter : IPresenter
    {
        private readonly CellModel _model;
        private readonly CellView _view;
        private readonly WorldViewDescriptions _worldViewDescriptions;

        public CellPresenter(CellModel model, CellView view, WorldViewDescriptions worldViewDescriptions)
        {
            _model = model;
            _view = view;
            _worldViewDescriptions = worldViewDescriptions;
        }
        
        public void Enable()
        {
            var tile = _worldViewDescriptions.SurfaceViewDescriptions.Get("ground");
            
            _view.Tilemap.SetTile(_model.Position, tile.TileAsset.editorAsset);
        }

        public void Disable()
        {
            _view.Tilemap.SetTile(_model.Position, null);
        }
    }
}