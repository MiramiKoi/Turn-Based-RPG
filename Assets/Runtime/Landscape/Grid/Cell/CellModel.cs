using System;
using Runtime.Common;
using Runtime.Descriptions.Locations.Environment;
using Runtime.Descriptions.Locations.Surface;
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
        public SurfaceDescription SurfaceDescription { get; }
        public EnvironmentDescription EnvironmentDescription { get; }
        public IndicationType IndicationType { get; private set; }

        public CellModel(int x, int y, SurfaceDescription surfaceDescription,
            EnvironmentDescription environmentDescription)
        {
            Position = new Vector2Int(x, y);
            SurfaceDescription = surfaceDescription;
            EnvironmentDescription = environmentDescription;
            IsOccupied = !(surfaceDescription.IsWalkable && environmentDescription.IsWalkable);
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