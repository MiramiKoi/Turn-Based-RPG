using System.Collections.Generic;
using Runtime.Extensions;
using UnityEngine;

namespace Runtime.Descriptions.Agents.Nodes
{
    public class AgentDecisionLink : AgentNode
    {
        private const string DescriptionIdKey = "description_id";

        public override string Type => "link";

        public string DescriptionId { get; set; } = string.Empty;

        public override NodeStatus Process(IWorldContext context, IControllable controllable)
        {
            Debug.Log($"ID: {DescriptionId}");

            return context.WorldDescription.AgentDecisionDescriptionCollection
                .Get(DescriptionId)
                .Process(context, controllable);
        }

        public override Dictionary<string, object> Serialize()
        {
            var dictionary = base.Serialize();
            dictionary[DescriptionIdKey] = DescriptionId;
            return dictionary;
        }

        public override void Deserialize(Dictionary<string, object> dictionary)
        {
            base.Deserialize(dictionary);

            DescriptionId = dictionary.GetString(DescriptionIdKey);
        }
    }
}