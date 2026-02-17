using Runtime.Descriptions.Locations;
using UnityEngine;

namespace Runtime.Landscape.Grid
{
    public class LocationModel
    {
        public readonly int AnchorX;
        public readonly int AnchorY;
        public readonly int Width;
        public readonly int Height;
        public Vector2Int? Entrance { get; private set; }
        public Vector2Int? Exit { get; private set; }
        public readonly int[,] SurfaceMatrix;
        public readonly int[,] EnvironmentMatrix;
        public readonly LocationDescription LocationDescription;

        public LocationModel(int anchorX, int anchorY, int width, int height, int[,] surfaceMatrix,
            int[,] environmentMatrix, LocationDescription locationDescription)
        {
            AnchorX = anchorX;
            AnchorY = anchorY;
            Width = width;
            Height = height;
            SurfaceMatrix = surfaceMatrix;
            EnvironmentMatrix = environmentMatrix;
            LocationDescription = locationDescription;
        }

        public void SetEntrance(Vector2Int? entrancePosition)
        {
            Entrance = entrancePosition;
        }

        public void SetExit(Vector2Int? exitPosition)
        {
            Exit = exitPosition;
        }
    }
}