using System.Linq;
using Runtime.Core;
using UnityEngine;

namespace Runtime.SpawnDirector.Rules
{
    public class SoloSpawnRule : SpawnRuleBase
    {
        private readonly Vector2Int? _spawnPosition;
        private int _respawnCounter;

        public SoloSpawnRule(
            World world,
            string descriptionId,
            int respawnDelaySteps,
            Vector2Int? spawnPosition = null)
            : base(world, descriptionId, respawnDelaySteps)
        {
            _spawnPosition = spawnPosition;
            _respawnCounter = RespawnDelaySteps;
        }

        public override void Run()
        {
            var hasAliveUnit = World.UnitCollection.Models.Values
                .Any(unit => unit.Description.Id == DescriptionId && !unit.IsDead);

            if (hasAliveUnit)
            {
                _respawnCounter = 0;
                return;
            }

            _respawnCounter++;

            if (_respawnCounter < RespawnDelaySteps)
                return;

            if (_spawnPosition != null)
            {
                
                SpawnAt((Vector2Int)_spawnPosition);
            }
            else
            {
                var randomAvailablePosition = World.GridModel.GetRandomAvailablePosition();
                if (randomAvailablePosition != null)
                {
                    
                    SpawnAt((Vector2Int)randomAvailablePosition);
                }
            }
            _respawnCounter = 0;
        }
    }
}