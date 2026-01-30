using System.Collections.Generic;
using Runtime.Common;
using Runtime.Core;
using Runtime.ViewDescriptions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Runtime.Landscape.Grid.Indication
{
    public class GridIndicationPresenter : IPresenter
    {
        private readonly GridIndicationView _view;
        private readonly World _world;
        private readonly WorldViewDescriptions _worldViewDescriptions;
        
        private readonly Dictionary<IndicationType, Tile> _indicationTiles = new();
        
        public GridIndicationPresenter(GridIndicationView view, World world, WorldViewDescriptions worldViewDescriptions)
        {
            _view = view;
            _world = world;
            _worldViewDescriptions = worldViewDescriptions;
        }
        
        public async void Enable()
        {
            var cellCursorTile = await _worldViewDescriptions.GridIndicationViewDescription.CellCursorAsset.LoadAssetAsync().Task;
            var routePointTile = await _worldViewDescriptions.GridIndicationViewDescription.RoutePointAsset.LoadAssetAsync().Task;
            _indicationTiles.Add(IndicationType.Null, null);
            _indicationTiles.Add(IndicationType.Cursor, cellCursorTile);
            _indicationTiles.Add(IndicationType.RoutePoint, routePointTile);
            
            _worldViewDescriptions.GridIndicationViewDescription.CellCursorAsset.ReleaseAsset();
            _worldViewDescriptions.GridIndicationViewDescription.RoutePointAsset.ReleaseAsset();
            
            foreach (var cell in _world.GridModel.Cells)
                cell.OnIndicationTypeChanged += HandleCellIndicationTypeChange;
        }

        public void Disable()
        {
            foreach (var cell in _world.GridModel.Cells)
                cell.OnIndicationTypeChanged -= HandleCellIndicationTypeChange;
        }

        private void Draw(Vector2Int position)
        {
            var indicationType = _world.GridModel.Cells[position.x, position.y].IndicationType;
            _view.Tilemap.SetTile(new Vector3Int(position.x, position.y, 0), _indicationTiles[indicationType]);
        }
        
        private void HandleCellIndicationTypeChange(Vector2Int position)
        {
            Draw(position);
        }
    }
}