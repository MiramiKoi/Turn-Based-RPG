using Runtime.Core;
using Runtime.Units;
using UnityEngine;

namespace Runtime.SpawnDirector.Rules
{
    public abstract class SpawnRuleBase : ISpawnRule
    {
        protected readonly World World;
        protected readonly string DescriptionId;
        protected readonly int RespawnDelaySteps;

        protected SpawnRuleBase(World world, string descriptionId, int respawnDelaySteps)
        {
            World = world;
            DescriptionId = descriptionId;
            RespawnDelaySteps = respawnDelaySteps;
        }

        public abstract void Run();

        protected UnitModel SpawnAt(Vector2Int position)
        {
            var unit = World.UnitCollection.Create(DescriptionId);
            unit.Movement.MoveTo(position);
            World.GridModel.TryPlace(unit, position);
            return unit;
        }

        protected void RemoveUnit(UnitModel unit)
        {
            if (unit.State.Position.HasValue)
            {
                World.GridModel.ReleaseCell(unit.State.Position.Value);
            }

            World.UnitCollection.Remove(unit.Id);
        }
    }
}
