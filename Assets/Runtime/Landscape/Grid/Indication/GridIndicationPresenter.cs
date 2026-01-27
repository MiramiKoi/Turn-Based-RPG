using Runtime.Common;
using Runtime.Landscape.Grid.Cell;
using Runtime.ViewDescriptions;
using UnityEngine;

namespace Runtime.Landscape.Grid.Indication
{
    public class GridIndicationPresenter : IPresenter
    {
        private readonly GridIndicationView _view;
        private readonly World _world;
        private readonly WorldViewDescriptions _worldViewDescriptions;
        private CellModel _currentCell;
        
        public GridIndicationPresenter(GridIndicationView view, World world, WorldViewDescriptions worldViewDescriptions)
        {
            _view = view;
            _world = world;
            _worldViewDescriptions = worldViewDescriptions;
        }
        
        public void Enable()
        {
            _world.GridInteractionModel.OnCellChanged += Update;
        }
        
        public void Disable()
        {
            _world.GridInteractionModel.OnCellChanged -= Update;
            Clear();
        }

        private void Update(CellModel cell)
        {
            if (_currentCell == cell)
            {
                return;
            }
            
            Clear();
            
            _currentCell = cell;
            
            Draw();
        }

        private void Draw()
        {
            if (_currentCell == null)
            {
                return;
            }

            var tile = _worldViewDescriptions._gridIndicationViewDescription.TileAsset.editorAsset;
            _view.Tilemap.SetTile(ToCellPos(_currentCell), tile);
        }

        private void Clear()
        {
            if (_currentCell == null)
            {
                return;
            }
            
            _view.Tilemap.SetTile(ToCellPos(_currentCell), null);
        }
        
        private Vector3Int ToCellPos(CellModel cell)
        {
            return new Vector3Int(cell.Position.x, cell.Position.y, 0);
        }
    }
}