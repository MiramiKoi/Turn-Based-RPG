using Runtime.Common;
using Runtime.Landscape.Grid.Cell;
using UnityEngine;

namespace Runtime.Landscape.Grid
{
    public class GridModel
    {
        public CellModel[,] Cells { get; }

        public GridModel()
        {
            Cells = new CellModel[GridConstants.Width, GridConstants.Height];
        
            for (var y = 0; y < GridConstants.Height; y++)
            {
                for (var x = 0; x < GridConstants.Width; x++)
                {
                    Cells[x, y] = new CellModel(x, y);
                }
            }
        }
        
        public CellModel GetCell(Vector2Int position) => Cells[position.x, position.y];

        public IUnit GetUnit(Vector2Int position) =>Cells[position.x, position.y].Unit;

        public bool CanPlace(Vector2Int position)
        {
            var cell = Cells[position.x, position.y];

            return !cell.IsOccupied;
        }

        public bool TryPlace(IUnit unit, Vector2Int position)
        {
            var cell = Cells[position.x, position.y];
            
            if (CanPlace(position))
            {
                cell.Occupied(unit);
                return true;
            }
            
            return false;
        }

        public void ReleaseCell(Vector2Int position)
        {
            var cell = Cells[position.x, position.y];
            
            cell.Release();
        }
    }
}
