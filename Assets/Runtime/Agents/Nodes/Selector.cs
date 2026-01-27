namespace Runtime.Agents.Nodes
{
    public class Selector : Node
    {
        public override NodeStatus Process()
        {
            if (CurrentChildIndex < Children.Count)
            {
                switch (Children[CurrentChildIndex].Process())
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