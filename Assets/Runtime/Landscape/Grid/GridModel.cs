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
        private readonly Dictionary<string, LocationModel> _locationModelCollection = new();

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

                var locationModel = new LocationModel
                {
                    Name = locationDescription.Key,
                    X = currentX,
                    Y = 0,
                    Width = width,
                    Height = height,
                    SurfaceMatrix = surfaceMatrix,
                    EnvironmentMatrix = environmentMatrix
                };

                _locationModelCollection[locationDescription.Key] = locationModel;

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

            foreach (var locationModel in _locationModelCollection.Values)
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

                    if (Cells[center.x + dx, center.y + dy] != null)
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

                    var globalX = locationModel.X + x;
                    var globalY = locationModel.Y + y;

                    Cells[globalX, globalY] =
                        new CellModel(globalX, globalY, surfaceDescription, environmentDescription);
                }
            }
        }
    }

    public class LocationModel
    {
        public string Name;
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public Vector2Int Entrance; // Координаты, на которые перемещается игрок при входе в локацию
        public Vector2Int Exit; // Куда ведет (координаты здания на другой локации)
        public int[,] SurfaceMatrix;
        public int[,] EnvironmentMatrix;
    }
}