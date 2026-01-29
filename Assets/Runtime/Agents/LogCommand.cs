using Runtime.Agents.Nodes;
using UnityEngine;

namespace Runtime.Agents
{
    public class LogCommand : IUnitCommand
    {
        public string Message { get; }

        public LogCommand(string message)
        {
            Message = message;
        }

        public NodeStatus Execute(IWorldContext context, IUnit unit)
        {
            Debug.Log($"Execute Log: {Message} ");
            
            return NodeStatus.Success;
        }
    }
}