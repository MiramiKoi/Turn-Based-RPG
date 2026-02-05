namespace Runtime.Descriptions.Agents.Nodes
{
    public class AgentDecisionRoot : AgentNode
    {
        public override string Type => "root";

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