namespace Runtime.Agents.Nodes
{
    public class BehaviorTree : Node
    {
        protected override string Type => "root";

        public override NodeStatus Process(IWorldContext context, IUnit unit)
        {
            while(CurrentChildIndex < Children.Count)
            {
                var status = Children[CurrentChildIndex].Process(context, unit);

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