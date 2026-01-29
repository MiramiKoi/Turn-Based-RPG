using UnityEngine;

namespace Runtime.Agents.Nodes
{
    public class AgentBehaviorTree : AgentNode
    {
        public override string Type => "root";

        public override NodeStatus Process(IWorldContext context, IUnit unit)
        {
            while (CurrentChildIndex < Children.Count)
            {
                var status = Children[CurrentChildIndex].Process(context, unit);

                if (status != NodeStatus.Success)
                {
                    Debug.Log(status);
                    return status;
                    
                }

                CurrentChildIndex++;
            }
            
            Debug.Log("Succses");

            return NodeStatus.Success;
        }
    }
}