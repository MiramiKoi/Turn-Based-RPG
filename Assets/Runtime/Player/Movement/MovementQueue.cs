using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Player.Movement
{
    public class MovementQueue
    {
        public bool HasSteps => _steps.Count > 0;
        public IReadOnlyCollection<Vector2Int> Steps => _steps;
            
        private readonly Queue<Vector2Int> _steps = new();

        public void SetPath(IReadOnlyList<Vector2Int> path)
        {
            _steps.Clear();
            if (path is not { Count: > 1 })
                return;

            for (var i = 1; i < path.Count; i++)
                _steps.Enqueue(path[i]);
        }

        public Vector2Int Dequeue() => _steps.Dequeue();

        public void Clear() => _steps.Clear();
    }
}