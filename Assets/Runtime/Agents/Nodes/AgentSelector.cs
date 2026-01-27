namespace Runtime.Agents.Nodes
{
    public class AgentSelector : AgentNode
    {
        protected override string Type => "selector";

        public override NodeStatus Process(IWorldContext context, IUnit unit)
        {
            if (CurrentChildIndex < Children.Count)
            {
                switch (Children[CurrentChildIndex].Process(context, unit))
                {
                    case NodeStatus.Running:
                        return NodeStatus.Running;
                    case NodeStatus.Success:
                        Reset();
                        return NodeStatus.Success;
                    default:
                        CurrentChildIndex++;
                        return NodeStatus.Running;
                }
            }
                            
            Reset();
            return NodeStatus.Failure;
        }
    }
}