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
        private LoadModel<TileBase> _surfaceTileLoadModel;
        private LoadModel<TileBase> _environmentTileLoadModel;

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
            var environmentView = _worldViewDescriptions.EnvironmentViewDescriptions.Get(_model.EnvironmentDescription.ViewId);            
            
            if (surfaceView == null)
            {
                return;
            }
            
            if (environmentView != null)
            {
                _environmentTileLoadModel = _world.AddressableModel.Load<TileBase>(environmentView.TileAsset.AssetGUID);
                await _environmentTileLoadModel.LoadAwaiter;
                _view.EnvironmentTilemap.SetTile(GridHelper.ToCellPos(_model.Position), _environmentTileLoadModel.Result);
            }

            _surfaceTileLoadModel = _world.AddressableModel.Load<TileBase>(surfaceView.TileAsset.AssetGUID);
            await _surfaceTileLoadModel.LoadAwaiter;
            _view.SurfacesTilemap.SetTile(GridHelper.ToCellPos(_model.Position), _surfaceTileLoadModel.Result);
        }

        public void Disable()
        {
            _world.AddressableModel.Unload(_surfaceTileLoadModel);
            _view.SurfacesTilemap.SetTile(GridHelper.ToCellPos(_model.Position), null);

            _world.AddressableModel.Unload(_environmentTileLoadModel);
            _view.EnvironmentTilemap.SetTile(GridHelper.ToCellPos(_model.Position), null);
        }
    }
}