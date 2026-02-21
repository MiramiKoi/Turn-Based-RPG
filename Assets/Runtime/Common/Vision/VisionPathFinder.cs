using System;
using Runtime.Landscape.Grid;
using UnityEngine;

namespace Runtime.Common.Vision
{
    public static class VisionPathFinder
    {
        public static bool Trace(GridModel grid, Vector2Int from, Vector2Int to, int radius)
        {
            if (!InVisionRadius(from, to, radius))
            {
                return false;
            }

            var steep = Math.Abs(to.y - from.y) > Math.Abs(to.x - from.x);

            var x0 = from.x;
            var y0 = from.y;
            var x1 = to.x;
            var y1 = to.y;

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

            var dx = x1 - x0;
            var dy = Math.Abs(y1 - y0);
            var error = dx / 2;
            var yStep = y0 < y1 ? 1 : -1;
            var y = y0;

            for (var x = x0; x <= x1; x++)
            {
                var gridX = steep ? y : x;
                var gridY = steep ? x : y;

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