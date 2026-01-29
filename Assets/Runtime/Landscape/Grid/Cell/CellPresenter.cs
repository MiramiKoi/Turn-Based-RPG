using Runtime.Common;
using Runtime.ViewDescriptions;

namespace Runtime.Landscape.Grid.Cell
{
    public class CellPresenter : IPresenter
    {
        private readonly CellModel _model;
        private readonly CellView _view;
        private readonly World _world;
        private readonly WorldViewDescriptions _worldViewDescriptions;

        public CellPresenter(CellModel model, CellView view, World world, WorldViewDescriptions worldViewDescriptions)
        {
            _model = model;
            _view = view;
            _world = world;
            _worldViewDescriptions = worldViewDescriptions;
        }

        public void Enable()
        {

            var tileView = _worldViewDescriptions.SurfaceViewDescriptions.Get(_model.SurfaceDescription.ViewId);
            if (tileView == null)
            {
                return;
            }

            _view.Tilemap.SetTile(GridHelper.ToCellPos(_model.Position), tileView.TileAsset.editorAsset);
        }

        public void Disable()
        {
            _view.Tilemap.SetTile(GridHelper.ToCellPos(_model.Position), null);
        }
    }
}