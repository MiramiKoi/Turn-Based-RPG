using System.Collections.Generic;
using Runtime.Common;
using Runtime.Descriptions;
using Runtime.Landscape.Grid.Cell;
using Runtime.Landscape.Grid.Indication;
using UnityEngine;

namespace Runtime.Landscape.Grid
{
    public class GridModel
    {
        public CellModel[,] Cells { get; }

        public GridModel(WorldDescription worldDescription)
        {
            var surfaceMatrix = worldDescription.SurfaceGenerationDescription.Generate();
            var environmentMatrix = worldDescription.EnvironmentGenerationDescription.Generate(surfaceMatrix);

            Cells = new CellModel[GridConstants.Width, GridConstants.Height];

            for (var y = 0; y < GridConstants.Height; y++)
            {
                for (var x = 0; x < GridConstants.Width; x++)
                {
                    var surface = surfaceMatrix[y, x].ToString();
                    var environment = environmentMatrix[y, x].ToString();

                    worldDescription.SurfaceCollection.Surfaces.TryGetValue(surface, out var surfaceDescription);
                    worldDescription.EnvironmentCollection.Environment.TryGetValue(environment,
                        out var environmentDescription);
                    Cells[x, y] = new CellModel(x, y, surfaceDescription, environmentDescription);
                }
            }
        }

        public CellModel GetCell(Vector2Int position)
        {
            return Cells[position.x, position.y];
        }

        public bool CanPlace(Vector2Int position)
        {
            if (IsInsideGrid(position))
            {
                var cell = Cells[position.x, position.y];
                return !cell.IsOccupied;
            }

            return false;
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

        public void SetIndication(IEnumerable<Vector2Int> cells, IndicationType indicationType)
        {
            foreach (var position in cells)
            {
                Cells[position.x, position.y].SetIndication(indicationType);
            }
        }
    }
}