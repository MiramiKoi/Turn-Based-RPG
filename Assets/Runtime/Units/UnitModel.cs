using System.Collections.Generic;
using Runtime.Common;
using Runtime.Descriptions;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Descriptions.Units;
using Runtime.Extensions;
using Runtime.ModelCollections;
using Runtime.Stats;
using Runtime.StatusEffects.Applier;
using Runtime.UI.Inventory;
using UnityEngine;

namespace Runtime.Units
{
    public abstract class UnitModel : IUnit, ISerializable
    {
        public string Id { get; }
        public int Health => (int)Stats["health"].Value;
        public bool IsDead => (int)Stats["health"].Value <= 0;
        public UnitDescription Description { get; }

        public UnitStateModel State { get; }
        public UnitCombatModel Combat { get; }
        public UnitMovementModel Movement { get; }
        public ActionBlockerModel ActionBlocker { get; }

        public StatModelCollection Stats { get; }
        public InventoryModel Inventory { get; }
        public StatusEffectApplierModel Effects { get; }

        public IReadOnlyDictionary<string, bool> Flags => _flags;

        private readonly Dictionary<string, bool> _flags = new();

        public UnitModel(string id, Vector2Int position, UnitDescription description, WorldDescription worldDescription)
        {
            Id = id;
            Description = description;

            Stats = new StatModelCollection(description.Stats);
            State = new UnitStateModel(position);

            ActionBlocker = new ActionBlockerModel();
            Movement = new UnitMovementModel(State, ActionBlocker);
            Combat = new UnitCombatModel(Stats, State);

            Inventory = new InventoryModel(description.InventorySize);
            Effects = new StatusEffectApplierModel(worldDescription);
        }

        public Dictionary<string, object> Serialize()
        {
            return new Dictionary<string, object>
            {
                { "id", Id },
                { "description_id", Description.Id },
                { "position", State.Position.Value.ToList() },
                { "direction", State.Direction.Value },
                { "stats", Stats.Serialize() },
                { "inventory", Inventory.Serialize() },
                { "active_effects", Effects.Serialize() },
                { "flags", _flags }
            };
        }

        public void SetFlag(string key, bool value)
        {
            _flags[key] = value;
        }
    }
}