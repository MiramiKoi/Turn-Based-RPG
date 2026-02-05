using System.Collections.Generic;
using Runtime.Stats;
using UniRx;
using UnityEngine;

namespace Runtime.Descriptions.Agents.Nodes
{
    public interface IControllable
    {
        IReadOnlyReactiveProperty<Vector2Int> Position { get; }
        IReadOnlyDictionary<string, bool> Flags { get; }
        
        IReadOnlyDictionary<string, Vector2Int> PointOfInterest { get; }
        
        StatModelCollection Stats { get; }
        
        void SetFlag(string key, bool value);
        
        void SetPointOfInterest(string key, Vector2Int value);
        
        Vector2Int GetPointOfInterest(string key);
    }
}