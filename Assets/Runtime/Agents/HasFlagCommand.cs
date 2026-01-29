using Runtime.Agents.Nodes;
using UnityEngine;

namespace Runtime.Agents
{
    public class HasFlagCommand : IUnitCommand
    {
        public string Flag { get; }

        public HasFlagCommand(string flag)
        {
            Flag = flag;
        }

        public NodeStatus Execute(IWorldContext context, IUnit unit)
        {
            if (unit.Flags.TryGetValue(Flag, out var flag))
            {
                Debug.Log($"{Flag}: {flag}");
                return flag ? NodeStatus.Success :  NodeStatus.Failure;  
            }
            
            return NodeStatus.Failure;
        }
    }
}