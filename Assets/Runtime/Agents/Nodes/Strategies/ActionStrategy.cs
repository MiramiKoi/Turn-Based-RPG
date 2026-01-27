using System;

namespace Runtime.Agents.Nodes
{
    public class ActionStrategy : ILeafStrategy
    {
        private readonly Action _action;

        public ActionStrategy(Action action)
        {
            _action = action;
        }

        public NodeStatus Process()
        {
            _action.Invoke();
            
            return NodeStatus.Success;
        }
    }
}