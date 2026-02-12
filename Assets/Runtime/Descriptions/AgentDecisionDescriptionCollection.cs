using System.Collections.Generic;
using Runtime.Descriptions.Agents.Nodes;

namespace Runtime.Descriptions
{
    public class AgentDecisionDescriptionCollection
    {
        public Dictionary<string, AgentDecisionDescription> Descriptions { get; }

        public AgentDecisionDescriptionCollection(Dictionary<string, object> data)
        {
            Descriptions = new Dictionary<string, AgentDecisionDescription>();

            foreach (var pair in data)
            {
                var effectData = pair.Value as Dictionary<string, object>;
                var description = new AgentDecisionDescription(effectData);

                Descriptions.Add(pair.Key, description);
            }
        }

        public AgentDecisionDescription Get(string id)
        {
            return Descriptions[id];
        }
    }
}