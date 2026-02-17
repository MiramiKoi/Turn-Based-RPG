using System.Collections.Generic;
using Runtime.Common;
using Runtime.Core;
using Runtime.Descriptions;
using Runtime.Landscape.Grid.Cell;
using Runtime.Landscape.Grid.Indication;
using UnityEngine;

namespace Runtime.Landscape.Grid
{
    public class GridModel
    {
        public CellModel[,] Cells { get; }
        public Dictionary<string, LocationModel> LocationModelCollection{ get; } = new() ;
        public int Width => Cells.GetLength(0);
        public int Height => Cells.GetLength(1);

        private readonly int _spacingAfterEveryLocation = 10;

        public GridModel(WorldDescription worldDescription)
        {
            var locationDescriptionCollection = worldDescription.LocationCollection;
            var totalWidth = 0;
            var maxHeight = 0;
            var currentX = 0;

            foreach (var locationDescription in locationDescriptionCollection.Locations)
            {
                var surfaceMatrix = worldDescription.SurfaceGenerationDescription.Generate(locationDescription.Value);
                var environmentMatrix =
                    worldDescription.EnvironmentGenerationDescription.Generate(locationDescription.Value,
                        surfaceMatrix);
                var width = surfaceMatrix.GetLength(1);
                var height = surfaceMatrix.GetLength(0);

                var locationModel = new LocationModel(currentX, 0, width, height, surfaceMatrix, environmentMatrix,
                    locationDescription.Value);
                
                if (locationDescription.Value.Entrance != null && locationDescription.Value.Exit != null)
                {
                    if (locationDescription.Value.Entrance.Position.HasValue)
                    {
                        locationModel.SetEntrance(locationDescription.Value.Entrance.Position.Value + new Vector2Int(currentX, 0));
                    }
                    else
                    {
                        locationModel.SetEntrance(locationDescription.Value.Entrance.Position);
                    }
                    if (locationDescription.Value.Exit.Position.HasValue)
                    {
                        locationModel.SetExit(locationDescription.Value.Exit.Position.Value + new Vector2Int(currentX, 0));
                    }
                    else
                    {
                        locationModel.SetExit(locationDescription.Value.Exit.Position);
                    }
                }
                
                LocationModelCollection[locationDescription.Key] = locationModel;

                totalWidth += width + _spacingAfterEveryLocation;
                maxHeight = Mathf.Max(maxHeight, height);
                currentX += width + _spacingAfterEveryLocation;
            }

            Cells = new CellModel[totalWidth, maxHeight];
            worldDescription.SurfaceCollection.Surfaces.TryGetValue("0", out var defaultSurfaceDescription);
            worldDescription.EnvironmentCollection.Environment.TryGetValue("0", out var defaultEnvironmentDescription);
            for (var x = 0; x < totalWidth; x++)
            {
                for (var y = 0; y < maxHeight; y++)
                {
                    Cells[x, y] = new CellModel(x, y, defaultSurfaceDescription, defaultEnvironmentDescription);
                }
            }

            foreach (var locationModel in LocationModelCollection.Values)
            {
                PlaceLocation(locationModel, worldDescription);
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
                return cell != null && !cell.IsOccupied;
            }

            return false;
        }

        public bool TryPlace(IUnit unit, Vector2Int position)
        {
            var cell = Cells[position.x, position.y];

            if (!CanPlace(position) || cell == null)
            {
                return false;
            }

            cell.Occupied(unit);
            return true;
        }

        public void ReleaseCell(Vector2Int position)
        {
            var cell = Cells[position.x, position.y];
            cell?.Release();
        }

        public bool IsInsideGrid(Vector2Int pos)
        {
            return pos is { x: >= 0, y: >= 0 } &&
                   pos.x < Cells.GetLength(0) &&
                   pos.y < Cells.GetLength(1);
        }

        public Vector2Int? GetRandomAvailablePosition()
        {
            const int maxAttempts = 100;
            for (var i = 0; i < maxAttempts; i++)
            {
                var x = Random.Range(0, GridConstants.Width);
                var y = Random.Range(0, GridConstants.Height);
                var pos = new Vector2Int(x, y);

                if (CanPlace(pos))
                {
                    return pos;
                }
            }

            return null;
        }

        public List<Vector2Int> GetNeighborAvailablePositions(Vector2Int center)
        {
            var list = new List<Vector2Int>();
            for (var dx = -1; dx <= 1; dx++)
            {
                for (var dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0)
                        continue;

                    if (IsInsideGrid(new Vector2Int(center.x + dx, center.y + dy)) && Cells[center.x + dx, center.y + dy] != null)
                    {
                        list.Add(new Vector2Int(center.x + dx, center.y + dy));
                    }
                }
            }

            return list;
        }

        public void SetIndication(IEnumerable<Vector2Int> cells, IndicationType indicationType)
        {
            foreach (var position in cells)
            {
                if (IsInsideGrid(position) && Cells[position.x, position.y] != null &&
                    !Cells[position.x, position.y].IsOccupied)
                {
                    Cells[position.x, position.y].SetIndication(indicationType);
                }
            }
        }

        private void PlaceLocation(LocationModel locationModel, WorldDescription worldDescription)
        {
            for (var y = 0; y < locationModel.Height; y++)
            {
                for (var x = 0; x < locationModel.Width; x++)
                {
                    var surface = locationModel.SurfaceMatrix[y, x].ToString();
                    var environment = locationModel.EnvironmentMatrix[y, x].ToString();

                    worldDescription.SurfaceCollection.Surfaces.TryGetValue(surface, out var surfaceDescription);
                    worldDescription.EnvironmentCollection.Environment.TryGetValue(environment,
                        out var environmentDescription);

                    var globalX = locationModel.AnchorX + x;
                    var globalY = locationModel.AnchorY + y;

                    Cells[globalX, globalY] =
                        new CellModel(globalX, globalY, surfaceDescription, environmentDescription);
                }
            }
        }
    }
}