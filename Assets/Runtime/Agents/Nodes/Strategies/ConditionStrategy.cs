using System;

namespace Runtime.Agents.Nodes
{
    public class ConditionStrategy : ILeafStrategy
    {
        private readonly Func<bool> _predicate;
        
        public ConditionStrategy(Func<bool> predicate)
        {
            _predicate = predicate;
        }

        public NodeStatus Process()
        {
            return _predicate() ? NodeStatus.Success : NodeStatus.Failure;
        }
    }
}