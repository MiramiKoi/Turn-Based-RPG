using System;
using System.Collections.Generic;
using Runtime.AsyncLoad;
using Runtime.Common;
using Runtime.Descriptions;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Descriptions.Units;
using Runtime.Stats;
using Runtime.StatusEffects.Collection;
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
        public StatusEffectModelCollection ActiveEffects { get; }

        public IReadOnlyDictionary<string, bool> Flags => _flags;
        public IReadOnlyDictionary<string, Vector2Int> PointOfInterest => _pointOfInterest;

        public string Id { get; }

        public int Health => (int)Stats["health"].Value;
        
        public bool IsDead => (int)Stats["health"].Value <= 0;
        
        public CustomAwaiter Awaiter { get; private set; }

        private readonly Dictionary<string, bool> _flags = new();

        private readonly Dictionary<string, Vector2Int> _pointOfInterest = new();
                
        public UnitModel(string id, Vector2Int position, UnitDescription description, WorldDescription worldDescription)
        {
            Description = description;
            Id = id;
            Stats = new StatModelCollection(Description.Stats);
            ActiveEffects = new StatusEffectModelCollection(worldDescription.StatusEffectCollection);

            MoveTo(position);
        }

        public void MoveTo(Vector2Int position)
        {
            if (CanMove())
            {
                Awaiter = new CustomAwaiter();

                var current = Position.Value;

                if (position.x != current.x)
                    Rotate(position.x < current.x ? UnitDirection.Left : UnitDirection.Right);

                _position.Value = position;
            }
        }

        public bool CanMove()
        {
            return !IsActionDisabled(UnitActionType.Move) && !IsActionDisabled(UnitActionType.All);
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
            if (!IsActionDisabled(UnitActionType.All))
            {
                var current = Position.Value;
                if (position.x != current.x)
                    Rotate(position.x < current.x ? UnitDirection.Left : UnitDirection.Right);
            
                return Math.Abs(current.x - position.x) <= Stats["attack_range"].Value &&
                       Math.Abs(current.y - position.y) <= Stats["attack_range"].Value;
            }
            return false;
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
        
        public void SetActionDisabled(UnitActionType action, bool disabled)
        {
            if (action == UnitActionType.All)
            {
                SetActionDisabled(UnitActionType.Move, disabled);
                SetActionDisabled(UnitActionType.Attack, disabled);
                return;
            }

            SetFlag("action_disabled: " + action.ToString().ToLowerInvariant(), disabled);
        }

        public bool IsActionDisabled(UnitActionType action)
        {
            if (action == UnitActionType.All)
                return IsActionDisabled(UnitActionType.Move) && IsActionDisabled(UnitActionType.Attack);

            var key = "action_disabled: " + action.ToString().ToLowerInvariant();
            return _flags.TryGetValue(key, out var disabled) && disabled;
        }

        public void ResetActionDisables()
        {
            SetActionDisabled(UnitActionType.All, false);
        }
    }
}