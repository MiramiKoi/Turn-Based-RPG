using System.Collections.Generic;
using Runtime.Common;
using Runtime.Landscape.Grid;
using UnityEngine;

namespace Runtime.Player.Movement
{
    public class GridPathfinder
    {
        private static readonly Vector2Int[] Directions =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        public List<Vector2Int> FindPath(
            GridModel grid,
            Vector2Int start,
            Vector2Int target)
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
                    return Reconstruct(cameFrom, current);

                foreach (var dir in Directions)
                {
                    var next = current + dir;

                    if (!grid.IsInsideGrid(next) || grid.GetCell(next).IsOccupied)
                        continue;

                    var tentative = gScore[current] + 1;

                    if (!gScore.TryGetValue(next, out var cost) || tentative < cost)
                    {
                        cameFrom[next] = current;
                        gScore[next] = tentative;
                        open.Enqueue(next, tentative + Heuristic(next, target));
                    }
                }
            }

            return null;
        }

        private int Heuristic(Vector2Int a, Vector2Int b)
            => Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);

        private List<Vector2Int> Reconstruct(
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