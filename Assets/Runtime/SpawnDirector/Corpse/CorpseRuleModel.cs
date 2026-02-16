using System.Collections.Generic;
using Runtime.Descriptions.SpawnDirector.Rules;
using Runtime.SpawnDirector.Rules;
using Runtime.Units;

namespace Runtime.SpawnDirector.Corpse
{
    public sealed class CorpseRuleModel
    {
        public readonly Dictionary<UnitModel, int> CorpseCounters = new();
        public readonly SpawnRuleModel SpawnRuleModel;
        
        private readonly CorpseSettingsDescription _description;

        public CorpseRuleModel(SpawnRuleModel spawnRuleModel, CorpseSettingsDescription description)
        {
            _description = description;
            SpawnRuleModel = spawnRuleModel;
        }

        public void RegisterCorpse(UnitModel unit)
        {
            if (!CorpseCounters.ContainsKey(unit))
            {
                CorpseCounters.Add(unit, _description.LifetimeSteps);
            }
        }
    }
}