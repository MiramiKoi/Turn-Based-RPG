using System.Collections.Generic;
using Runtime.Agents.Nodes;
using Runtime.AsyncLoad;
using Runtime.Common;
using Runtime.Descriptions.Units;
using Runtime.Stats;
using UniRx;
using UnityEngine;

namespace Runtime.Units
{
    public class UnitModel : IUnit, IControllable
    {
        public UnitDescription Description { get; }
        
        private readonly ReactiveProperty<Vector2Int> _position = new ();
        public IReadOnlyReactiveProperty<Vector2Int> Position => _position;
        
        private readonly ReactiveProperty<UnitDirection> _direction = new ();
        public IReadOnlyReactiveProperty<UnitDirection> Direction => _direction;
        
        public StatModelCollection Stats { get; }

        public IReadOnlyDictionary<string, IUnitCommand> Commands => _commands;
        public IReadOnlyDictionary<string, bool> Flags => _flags;

        public string Id { get; }

        public int Health => Stats["health"];

        public CustomAwaiter Awaiter { get; private set; }

        private readonly Dictionary<string, IUnitCommand> _commands = new();

        private readonly Dictionary<string, bool> _flags = new();


        public UnitModel(string id, UnitDescription description, Vector2Int position)
        {
            Description = description;
            Id = id;
            Stats = new StatModelCollection(Description.Stats);
            
            MoveTo(position);
        }

        public void RegisterCommand(string key, IUnitCommand command)
        {
            _commands[key] = command;
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

        public void Rotate(UnitDirection direction)
        {
            _direction.Value = direction;
        }
    }
}