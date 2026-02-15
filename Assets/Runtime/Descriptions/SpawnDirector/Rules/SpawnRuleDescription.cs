using System.Collections.Generic;
using Runtime.Extensions;

namespace Runtime.Descriptions.SpawnDirector.Rules
{
    public class SpawnRuleDescription : Description
    {
        public string UnitDescriptionId { get; }
        public string RuleType { get; }
        public int? Count { get; }

        public SpawnSettingsDescription Spawn { get; }
        public CorpseSettingsDescription Corpse { get; }
        public RespawnSettingsDescription Respawn { get; }

        public SpawnRuleDescription(string id, Dictionary<string, object> data)
            : base(id)
        {
            UnitDescriptionId = data.GetString("unit_description");
            RuleType = data.GetString("rule_type");

            if (RuleType == "population")
                Count = data.GetInt("count");

            Spawn = new SpawnSettingsDescription(data.GetNode("spawn"));
            Corpse = new CorpseSettingsDescription(data.GetNode("corpse"));
            Respawn = new RespawnSettingsDescription(data.GetNode("respawn"));
        }
    }
}