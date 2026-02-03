using System.Collections.Generic;
using Runtime.AsyncLoad;
using Runtime.Common;
using Runtime.Core;
using Runtime.ViewDescriptions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Runtime.Landscape.Grid.Indication
{
    public class GridIndicationPresenter : IPresenter
    {
        private readonly GridView _view;
        private readonly World _world;
        private readonly WorldViewDescriptions _worldViewDescriptions;
        
        private readonly Dictionary<IndicationType, Tile> _indicationTiles = new();
        private LoadModel<Tile> _cellCursorLoadModel;
        private LoadModel<Tile> _routePointLoadModel;
        
        public GridIndicationPresenter(GridView view, World world, WorldViewDescriptions worldViewDescriptions)
        {
            _view = view;
            _world = world;
            _worldViewDescriptions = worldViewDescriptions;
        }
        
        public async void Enable()
        {
            _cellCursorLoadModel = _world.AddressableModel.Load<Tile>(_worldViewDescriptions.GridIndicationViewDescription.CellCursorAsset.AssetGUID);
            await _cellCursorLoadModel.LoadAwaiter;
            var cellCursorTile = _cellCursorLoadModel.Result;
            
            _routePointLoadModel = _world.AddressableModel.Load<Tile>(_worldViewDescriptions.GridIndicationViewDescription.RoutePointAsset.AssetGUID);
            await _routePointLoadModel.LoadAwaiter;
            var routePointTile = _routePointLoadModel.Result;
            
            _indicationTiles.Add(IndicationType.Null, null);
            _indicationTiles.Add(IndicationType.Cursor, cellCursorTile);
            _indicationTiles.Add(IndicationType.RoutePoint, routePointTile);
            
            foreach (var cell in _world.GridModel.Cells)
                cell.OnIndicationTypeChanged += HandleCellIndicationTypeChange;
        }

        public void Disable()
        {
            _world.AddressableModel.Unload(_cellCursorLoadModel);
            _world.AddressableModel.Unload(_routePointLoadModel);
            foreach (var cell in _world.GridModel.Cells)
                cell.OnIndicationTypeChanged -= HandleCellIndicationTypeChange;
        }

        private void Draw(Vector2Int position)
        {
            var indicationType = _world.GridModel.Cells[position.x, position.y].IndicationType;
            _view.IndicationTilemap.SetTile(GridHelper.ToCellPos(position), _indicationTiles[indicationType]);
        }
        
        private void HandleCellIndicationTypeChange(Vector2Int position)
        {
            Draw(position);
        }
    }
}