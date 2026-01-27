namespace Runtime.Agents.Nodes
{
    public class BehaviorTree : Node
    {
        public BehaviorTree()
        {
            
        }
        
        public override NodeStatus Process()
        {
            while(CurrentChildIndex < Children.Count)
            {
                var status = Children[CurrentChildIndex].Process();

                if (status != NodeStatus.Success)
                {
                    return status;
                }

                CurrentChildIndex++;
            }
            
            return NodeStatus.Success;
        }
    }
}