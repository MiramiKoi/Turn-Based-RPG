using Runtime.Landscape.Grid.Cell;
using UnityEngine;

namespace Runtime.Landscape.Grid
{
    public static class GridHelper
    {
        public static Vector3Int ToCellPos(CellModel cell)
        {
            return new Vector3Int(cell.Position.x, cell.Position.y, 0);
        }
        
        public static Vector3Int ToCellPos(Vector2Int cellPos)
        {
            return new Vector3Int(cellPos.x, cellPos.y, 0);
        }
    }
}