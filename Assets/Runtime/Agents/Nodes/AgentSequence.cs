namespace Runtime.Agents.Nodes
{
    public class AgentSequence : AgentNode
    {
        public override string Type => "sequence";

        public override NodeStatus Process(IWorldContext context, IUnit unit)
        {
            if (CurrentChildIndex < Children.Count)
            {
                var currentStatus = Children[CurrentChildIndex].Process(context, unit);

                switch (currentStatus)
                {
                    case NodeStatus.Running:
                        return NodeStatus.Running;
                    case NodeStatus.Failure:
                        Reset();
                        return NodeStatus.Failure;
                    default:
                        CurrentChildIndex++;
                        var isLastChild = CurrentChildIndex == Children.Count;
                        return isLastChild ? NodeStatus.Success : NodeStatus.Running;
                }
            }
            
            Reset();
            return NodeStatus.Success;
        }
    }
}