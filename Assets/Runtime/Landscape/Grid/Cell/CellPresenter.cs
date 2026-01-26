using Runtime.Common;
using Runtime.ViewDescriptions;
using UnityEngine;

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
            var tilePosition = new Vector3Int(_model.Position.x, _model.Position.y, 0);
            
            _view.Tilemap.SetTile(tilePosition, tile.TileAsset.editorAsset);
        }

        public void Disable()
        {
            var tilePosition = new Vector3Int(_model.Position.x, _model.Position.y, 0);
            
            _view.Tilemap.SetTile(tilePosition, null);
        }
    }
}