using UnityEngine;

namespace Runtime.Descriptions.Agents.Nodes
{
    public class AgentSequence : AgentNode
    {
        public override string Type => "sequence";

        public override NodeStatus Process(IWorldContext context, IControllable controllable)
        {
            foreach (var child in Children)
            {
                if (child.Process(context, controllable) != NodeStatus.Success)
                    return NodeStatus.Failure;
            }

            return NodeStatus.Success;
        }
    }
}