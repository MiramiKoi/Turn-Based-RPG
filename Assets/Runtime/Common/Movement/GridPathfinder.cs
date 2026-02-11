using System.Collections.Generic;
using Runtime.Landscape.Grid;
using UnityEngine;

namespace Runtime.Common.Movement
{
    public static class GridPathfinder
    {
        private static readonly Vector2Int[] Directions =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right,
            new(1, 1),
            new(-1, 1),
            new(1, -1),
            new(-1, -1)
        };

        public static bool FindPath(
            GridModel grid,
            Vector2Int start,
            Vector2Int target,
            out List<Vector2Int> path)
        {
            var open = new SimplePriorityQueue<Vector2Int>();
            var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
            var gScore = new Dictionary<Vector2Int, int>
            {
                [start] = 0
            };

            open.Enqueue(start, Heuristic(start, target));

            while (open.Count > 0)
            {
                var current = open.Dequeue();
                if (current == target)
                {
                    path = Reconstruct(cameFrom, current);
                    return true;
                }

                foreach (var direction in Directions)
                {
                    var next = current + direction;

                    if (grid.IsInsideGrid(next) && (next == target || !grid.GetCell(next).IsOccupied))
                    {
                        var stepCost = direction.x != 0 && direction.y != 0 ? 14 : 10;
                        var tentative = gScore[current] + stepCost;

                        if (!gScore.TryGetValue(next, out var cost) || tentative < cost)
                        {
                            cameFrom[next] = current;
                            gScore[next] = tentative;
                            open.Enqueue(next, tentative + Heuristic(next, target));
                        }
                    }
                }
            }

            path = null;
            return false;
        }

        private static int Heuristic(Vector2Int a, Vector2Int b)
        {
            var dx = Mathf.Abs(a.x - b.x);
            var dy = Mathf.Abs(a.y - b.y);

            return 14 * Mathf.Min(dx, dy) + 10 * Mathf.Abs(dx - dy);
        }

        private static List<Vector2Int> Reconstruct(
            Dictionary<Vector2Int, Vector2Int> cameFrom,
            Vector2Int current)
        {
            var path = new List<Vector2Int> { current };
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Add(current);
            }

            path.Reverse();
            return path;
        }
    }
}