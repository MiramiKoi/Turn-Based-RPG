using Runtime.Common;
using Runtime.ViewDescriptions;

namespace Runtime.Landscape.Grid.Cell
{
    public class CellPresenter : IPresenter
    {
        private readonly CellModel _model;
        private readonly GridView _view;
        private readonly WorldViewDescriptions _worldViewDescriptions;

        public CellPresenter(CellModel model, GridView view, WorldViewDescriptions worldViewDescriptions)
        {
            _model = model;
            _view = view;
            _worldViewDescriptions = worldViewDescriptions;
        }

        public void Enable()
        {
            var surfaceView = _worldViewDescriptions.SurfaceViewDescriptions.Get(_model.SurfaceDescription.ViewId);
            
            if (surfaceView == null)
            {
                return;
            }

            _view.SurfacesTilemap.SetTile(GridHelper.ToCellPos(_model.Position), surfaceView.TileAsset.editorAsset);
        }

        public void Disable()
        {
            _view.SurfacesTilemap.SetTile(GridHelper.ToCellPos(_model.Position), null);
        }
    }
}