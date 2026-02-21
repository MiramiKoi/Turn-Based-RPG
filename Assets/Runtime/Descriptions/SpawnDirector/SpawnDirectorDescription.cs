using System.Collections.Generic;
using Runtime.Descriptions.SpawnDirector.Rules;
using Runtime.Extensions;

namespace Runtime.Descriptions.SpawnDirector
{
    public class SpawnDirectorDescription
    {
        public IReadOnlyDictionary<string, SpawnRuleDescription> Rules { get; }

        public SpawnDirectorDescription(Dictionary<string, object> data)
        {
            var rulesData = data.GetNode("spawn_rules");

            var rules = new Dictionary<string, SpawnRuleDescription>();

            foreach (var (ruleId, rule) in rulesData)
            {
                var ruleDict = (Dictionary<string, object>)rule;

                var ruleDescription = new SpawnRuleDescription(ruleId, ruleDict);
                rules.Add(ruleId, ruleDescription);
            }

            Rules = rules;
        }
        
        public SpawnRuleDescription Get(string id)
        {
            return Rules[id];
        }
    }
}