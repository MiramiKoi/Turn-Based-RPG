using System;
using Runtime.Landscape.Grid;
using UnityEngine;

namespace Runtime.Common.Vision
{
    public class VisionPathFinder
    {
        public static bool Trace(GridModel grid, Vector2Int from, Vector2Int to, int radius)
        {
            if (!InVisionRadius(from, to, radius))
            {
                return false;
            }

            bool steep = Math.Abs(to.y - from.y) > Math.Abs(to.x - from.x);

            int x0 = from.x;
            int y0 = from.y;
            int x1 = to.x;
            int y1 = to.y;

            if (steep)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }

            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            int dx = x1 - x0;
            int dy = Math.Abs(y1 - y0);
            int error = dx / 2;
            int yStep = (y0 < y1) ? 1 : -1;
            int y = y0;

            for (int x = x0; x <= x1; x++)
            {
                int gridX = steep ? y : x;
                int gridY = steep ? x : y;

                if (!(gridX == from.x && gridY == from.y))
                {
                    if (!grid.IsInsideGrid(new Vector2Int(gridX, gridY)))
                        return false;

                    if (grid.GetCell(new Vector2Int(gridX, gridY)).EnvironmentDescription.BlockVision)
                        return false;
                }

                error -= dy;
                if (error < 0)
                {
                    y += yStep;
                    error += dx;
                }
            }

            return true;
        }


        private static void Swap(ref int a, ref int b)
        {
            (a, b) = (b, a);
        }

        private static bool InVisionRadius(Vector2Int from, Vector2Int to, int radius)
        {
            return (to - from).sqrMagnitude <= radius * radius;
        }
    }
}