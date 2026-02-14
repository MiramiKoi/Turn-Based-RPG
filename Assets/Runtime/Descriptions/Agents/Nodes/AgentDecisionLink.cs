using System.Collections.Generic;
using Runtime.Extensions;

namespace Runtime.Descriptions.Agents.Nodes
{
    public class AgentDecisionLink : AgentNode
    {
        private const string DescriptionIdKey = "description_id";
            
        public override string Type => "link";
        
        public string DescriptionId { get; set; } = string.Empty;
        
        public override NodeStatus Process(IWorldContext context, IControllable controllable)
        {
            return context.WorldDescription.AgentDecisionDescriptionCollection.
                Descriptions.TryGetValue(DescriptionId, out var link)
                ? link.Process(context, controllable) : NodeStatus.Failure;
        }

        public override Dictionary<string, object> Serialize()
        {
            var dictionary = base.Serialize();
            dictionary[DescriptionIdKey] =  DescriptionId;
            return dictionary;
        }

        public override void Deserialize(Dictionary<string, object> dictionary)
        {
            base.Deserialize(dictionary);
            
            DescriptionId = dictionary.GetString(DescriptionIdKey);
        }
    }
}