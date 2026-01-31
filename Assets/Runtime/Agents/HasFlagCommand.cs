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

        public NodeStatus Execute(IWorldContext context, IControllable controllable)
        {
            if (controllable.Flags.TryGetValue(Flag, out var flag))
            {
                return flag ? NodeStatus.Success :  NodeStatus.Failure;  
            }
            
            return NodeStatus.Failure;
        }
    }
}