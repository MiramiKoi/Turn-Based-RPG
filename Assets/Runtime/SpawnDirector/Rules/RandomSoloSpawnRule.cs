using Runtime.Core;
using Runtime.CustomAsync;
using UnityEngine;

namespace Runtime.SpawnDirector.Rules
{
    public class RandomSoloSpawnRule : SpawnRuleBase
    {
        public RandomSoloSpawnRule(World world, string descriptionId, int respawnDelaySteps) : base(world, descriptionId, respawnDelaySteps)
        {
        }

        public override async void Run()
        {
            
        }
    }
}