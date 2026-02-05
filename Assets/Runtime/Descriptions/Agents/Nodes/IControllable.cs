using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Runtime.Descriptions.Agents.Nodes
{
    public interface IControllable
    {
        public IReadOnlyReactiveProperty<Vector2Int> Position { get; }
        IReadOnlyDictionary<string, bool> Flags { get; }
        
        IReadOnlyDictionary<string, Vector2Int> PointOfInterest { get; }
        
        public void SetFlag(string key, bool value);
        
        public void SetPointOfInterest(string key, Vector2Int value);
        
        public Vector2Int GetPointOfInterest(string key);
    }
}