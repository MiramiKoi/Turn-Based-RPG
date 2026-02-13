using Runtime.Core;
using Runtime.Units;
using UnityEngine;

namespace Runtime.SpawnDirector.Rules
{
    public class SoloSpawnRule : SpawnRuleBase
    {
        private UnitModel _unitModel;
        private readonly Vector2Int? _spawnPosition;

        public SoloSpawnRule(
            World world,
            string descriptionId,
            Vector2Int? spawnPosition = null)
            : base(world, descriptionId)
        {
            _spawnPosition = spawnPosition;
        }

        public override void Run()
        {
            if (_unitModel == null)
            {
                if (_spawnPosition != null)
                {
                    _unitModel = SpawnAt((Vector2Int)_spawnPosition);
                }
                else
                {
                    var randomAvailablePosition = World.GridModel.GetRandomAvailablePosition();
                    if (randomAvailablePosition != null)
                    {
                    
                        _unitModel = SpawnAt((Vector2Int)randomAvailablePosition);
                    }
                }
            }
        }
    }
}