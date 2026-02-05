namespace Runtime.Agents.Nodes
{
    public class AgentSelector : AgentNode
    {
        public override string Type => "selector";

        public override NodeStatus Process(IWorldContext context, IControllable controllable)
        {
            foreach (var child in Children)
            {
                if (child.Process(context, controllable) == NodeStatus.Success)
                    return NodeStatus.Success;
            }

            return NodeStatus.Failure;
        }
    }
}