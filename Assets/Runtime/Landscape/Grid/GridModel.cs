using Runtime.Common;
using Runtime.Descriptions;
using Runtime.Descriptions.Surface;
using Runtime.Landscape.Grid.Cell;
using UnityEngine;

namespace Runtime.Landscape.Grid
{
    public class GridModel
    {
        public CellModel[,] Cells { get; }

        public GridModel(int[,] matrix, SurfaceDescriptionCollection surfaceDescriptionCollection)
        {
            Cells = new CellModel[GridConstants.Width, GridConstants.Height];
        
            for (var y = 0; y < GridConstants.Height; y++)
            {
                for (var x = 0; x < GridConstants.Width; x++)
                {
                    var surface = matrix[x, y].ToString();
                    
                    surfaceDescriptionCollection.Surfaces.TryGetValue(surface, out var description);
                    Cells[x, y] = new CellModel(x, y, description);
                }
            }
        }
        
        public CellModel GetCell(Vector2Int position) => Cells[position.x, position.y];

        public bool CanPlace(Vector2Int position)
        {
            var cell = Cells[position.x, position.y];
            return !cell.IsOccupied;
        }

        public bool TryPlace(IUnit unit, Vector2Int position)
        {
            var cell = Cells[position.x, position.y];
            
            if (!CanPlace(position))
            {
                return false;
            }
            
            cell.Occupied(unit);
            return true;
        }

        public void ReleaseCell(Vector2Int position)
        {
            var cell = Cells[position.x, position.y];
            cell.Release();
        }
        
        public bool IsInsideGrid(Vector2Int pos)
        {
            return pos is { x: >= 0 and < GridConstants.Width, y: >= 0 and < GridConstants.Height };
        }
    }
}
