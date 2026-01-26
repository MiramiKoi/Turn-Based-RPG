using Runtime.Common;
using UnityEngine;

namespace Runtime.Landscape.Grid.Cell
{
    public class CellModel
    {
        public Vector3Int Position { get; }
        public IUnit Unit { get; private set; }
        public bool IsOccupied { get; private set; }

        public CellModel(int x, int y)
        {
            Position = new Vector3Int(x, y, 0);
        }
        
        public void Occupied(IUnit unit)
        {
            Unit = unit;
            IsOccupied = true;
        }

        public void Occupied()
        {
            IsOccupied = true;
        }

        public void Release()
        {
            Unit = null;
            IsOccupied = false;
        }
    }
}