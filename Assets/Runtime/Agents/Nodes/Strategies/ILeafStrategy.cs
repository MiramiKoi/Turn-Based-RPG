namespace Runtime.Agents.Nodes
{
    public interface ILeafStrategy
    {
        NodeStatus Process();

        void Reset()
        {
            
        }
    }
}