using System.Collections.Generic;
using System.Linq;
using Runtime.Descriptions.SpawnDirector.Rules;
using Runtime.Units;

namespace Runtime.SpawnDirector.Rules
{
    public class SpawnRuleModel
    {
        public readonly SpawnRuleDescription Description;

        public readonly List<UnitModel> Units = new();

        public int RespawnCounter;

        public SpawnRuleModel(SpawnRuleDescription description)
        {
            Description = description;
        }

        public int AliveCount()
        {
            return Units.Count(unit => unit != null);
        }
    }
}