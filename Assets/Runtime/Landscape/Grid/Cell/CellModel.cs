using System;
using Runtime.Common;
using Runtime.Landscape.Grid.Indication;
using UnityEngine;

namespace Runtime.Landscape.Grid.Cell
{
    public class CellModel
    {
        public event Action<Vector2Int> OnIndicationTypeChanged;
        public Vector2Int Position { get; }
        public IUnit Unit { get; private set; }
        public bool IsOccupied { get; private set; }
        public IndicationType IndicationType { get; private set; }

        public CellModel(int x, int y)
        {
            Position = new Vector2Int(x, y);
        }
        
        public void Occupied(IUnit unit)
        {
            Unit = unit;
            IsOccupied = true;
            SetIndication(IndicationType.Null);
        }

        public void Release()
        {
            Unit = null;
            IsOccupied = false;
            SetIndication(IndicationType.Null);
        }
        
        public void SetIndication(IndicationType indicationType)
        {
            IndicationType = indicationType;
            OnIndicationTypeChanged?.Invoke(Position);
        }
    }
}