using Runtime.Common;
using Runtime.Descriptions.Units;
using Runtime.Stats;
using UniRx;
using UnityEngine;

namespace Runtime.Units
{
    public class UnitModel : IUnit
    {
        public UnitDescription Description { get; }
        
        private readonly ReactiveProperty<Vector2Int> _position = new ();
        public IReadOnlyReactiveProperty<Vector2Int> Position => _position;

        public StatModelCollection Stats { get; private set; }
        
        public string Id { get; }
        public int Health => Stats["health"];
        
        public UnitModel(string id, UnitDescription description, Vector2Int position)
        {
            Description = description;
            Id = id;
            Stats = new StatModelCollection(Description.Stats);
            
            MoveTo(position);
        }

        public void MoveTo(Vector2Int position)
        {
            _position.Value =  position;
        }
    }
}