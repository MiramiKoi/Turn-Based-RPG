using Runtime.AsyncLoad;
using Runtime.Common;
using Runtime.Core;
using Runtime.ViewDescriptions;
using UnityEngine.Tilemaps;

namespace Runtime.Landscape.Grid.Cell
{
    public class CellPresenter : IPresenter
    {
        private readonly CellModel _model;
        private readonly GridView _view;
        private readonly WorldViewDescriptions _worldViewDescriptions;
        private readonly World _world;
        private LoadModel<TileBase> _loadModel;

        public CellPresenter(CellModel model, GridView view, World world, WorldViewDescriptions worldViewDescriptions)
        {
            _model = model;
            _view = view;
            _worldViewDescriptions = worldViewDescriptions;
            _world = world;
        }

        public async void Enable()
        {
            var surfaceView = _worldViewDescriptions.SurfaceViewDescriptions.Get(_model.SurfaceDescription.ViewId);
            
            if (surfaceView == null)
            {
                return;
            }

            _loadModel = _world.AddressableModel.Load<TileBase>(surfaceView.TileAsset.AssetGUID);
            await _loadModel.LoadAwaiter;
            _view.SurfacesTilemap.SetTile(GridHelper.ToCellPos(_model.Position), _loadModel.Result);
        }

        public void Disable()
        {
            _world.AddressableModel.Unload(_loadModel);
            _view.SurfacesTilemap.SetTile(GridHelper.ToCellPos(_model.Position), null);
        }
    }
}