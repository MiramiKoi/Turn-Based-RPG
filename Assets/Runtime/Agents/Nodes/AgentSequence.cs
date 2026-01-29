namespace Runtime.Agents.Nodes
{
    public class AgentSequence : AgentNode
    {
        public override string Type => "sequence";

        public override NodeStatus Process(IWorldContext context, IUnit unit)
        {
            foreach (var child in Children)
            {
                if (child.Process(context, unit) != NodeStatus.Success)
                    return NodeStatus.Failure;
            }

            return NodeStatus.Success;
        }
    }
}