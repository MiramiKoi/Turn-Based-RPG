using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Descriptions.Agents.Nodes
{
    public sealed class AgentDecisionDescription : AgentNode
    {
        public override string Type => "root";

        public AgentDecisionDescription()
        {
            
        }
        
        public AgentDecisionDescription(Dictionary<string, object> data)
        {
            Deserialize(data);
        }
        
        public override NodeStatus Process(IWorldContext context, IControllable controllable)
        {
            foreach (var child in Children)
            {
                var status = child.Process(context, controllable);

                if (status == NodeStatus.Success)
                {
                    return NodeStatus.Success;
                }
            }
            
            return NodeStatus.Failure;
        }
    }
}