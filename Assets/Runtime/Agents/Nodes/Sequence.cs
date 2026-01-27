namespace Runtime.Agents.Nodes
{
    public class Sequence : Node
    {
        public override NodeStatus Process()
        {
            if (CurrentChildIndex < Children.Count)
            {
                var currentStatus = Children[CurrentChildIndex].Process();

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