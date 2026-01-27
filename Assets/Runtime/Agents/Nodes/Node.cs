using System.Collections.Generic;

namespace Runtime.Agents.Nodes
{
    public abstract class Node
    {
        protected List<Node> Children => _child;

        protected int CurrentChildIndex { get; set; } = 0;
        
        private readonly List<Node> _child = new();

        public void AddChild(Node child)
        {
            _child.Add(child);
        }

        public abstract NodeStatus Process();

        public virtual void Reset()
        {
            CurrentChildIndex = 0;
            
            foreach (var child in _child)
            {
                child.Reset();
            }
        }
}
}