namespace Runtime.Agents.Nodes
{
    public class Leaf : Node
    {
        private readonly ILeafStrategy _strategy;

        public Leaf(ILeafStrategy strategy)
        {
            _strategy = strategy;
        }

        public override NodeStatus Process()
        {
            return _strategy.Process();
        }

        public override void Reset()
        {
            _strategy.Reset();
        }
    }
}