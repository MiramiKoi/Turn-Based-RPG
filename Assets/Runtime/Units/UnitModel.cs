using System;
using System.Collections.Generic;
using Runtime.AsyncLoad;
using Runtime.Common;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Descriptions.Units;
using Runtime.Stats;
using UniRx;
using UnityEngine;

namespace Runtime.Units
{
    public class UnitModel : IUnit, IControllable
    {
        public event Action OnAttacked;
        public event Action OnDamaging;
        
        public UnitDescription Description { get; }
        
        private readonly ReactiveProperty<Vector2Int> _position = new ();
        public IReadOnlyReactiveProperty<Vector2Int> Position => _position;
        
        private readonly ReactiveProperty<UnitDirection> _direction = new ();
        public IReadOnlyReactiveProperty<UnitDirection> Direction => _direction;
        
        public StatModelCollection Stats { get; }

        public IReadOnlyDictionary<string, bool> Flags => _flags;
        public IReadOnlyDictionary<string, Vector2Int> PointOfInterest { get; private set; }

        public string Id { get; }

        public int Health => (int)Stats["health"].Value;
        
        public bool IsDead => (int)Stats["health"].Value <= 0;

        public CustomAwaiter Awaiter { get; private set; }

        private readonly Dictionary<string, bool> _flags = new();

        private readonly Dictionary<string, Vector2Int> _pointOfInterest = new();
                
        public UnitModel(string id, UnitDescription description, Vector2Int position)
        {
            Description = description;
            Id = id;
            Stats = new StatModelCollection(Description.Stats);
            
            MoveTo(position);
        }

        public void MoveTo(Vector2Int position)
        {
            Awaiter = new CustomAwaiter();
            
            var current = Position.Value;
         
            if (position.x != current.x)
                Rotate(position.x < current.x ? UnitDirection.Left : UnitDirection.Right);
            
            _position.Value =  position;
        }

        public void SetFlag(string key, bool value)
        {
            _flags[key] = value;
        }

        public void SetPointOfInterest(string key, Vector2Int value)
        {
            _pointOfInterest[key] = value;
        }

        public Vector2Int GetPointOfInterest(string key)
        {
            return _pointOfInterest[key];
        }

        public void Rotate(UnitDirection direction)
        {
            _direction.Value = direction;
        }
        
        public float GetDamage()
        {
            Awaiter = new CustomAwaiter();
            OnAttacked?.Invoke();
            
            return Stats["attack_damage"].Value;
        }

        public bool CanAttack(Vector2Int position)
        { 
            var current = Position.Value;
            if (position.x != current.x)
                Rotate(position.x < current.x ? UnitDirection.Left : UnitDirection.Right);
            
            return Math.Abs(current.x - position.x) <= Stats["attack_range"].Value &&
                   Math.Abs(current.y - position.y) <= Stats["attack_range"].Value;
        }

        public void TakeDamage(float damage)
        {
            Awaiter = new CustomAwaiter();
            OnDamaging?.Invoke();
            
            Stats["health"].ChangeValue(-damage);
        }

        public void Await()
        {
            Awaiter = new CustomAwaiter();
        }
    }
}