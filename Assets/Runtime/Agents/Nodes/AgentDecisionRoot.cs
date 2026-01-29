using UnityEngine;

namespace Runtime.Agents.Nodes
{
    public class AgentDecisionRoot : AgentNode
    {
        public override string Type => "root";

        public override NodeStatus Process(IWorldContext context, IUnit unit)
        {
            foreach (var child in Children)
            {
                var status = child.Process(context, unit);

                if (status == NodeStatus.Success)
                {
                    return NodeStatus.Success;
                }
            }
            
            return NodeStatus.Failure;
        }
    }
}