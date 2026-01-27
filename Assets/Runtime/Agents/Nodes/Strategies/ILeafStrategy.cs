namespace Runtime.Agents.Nodes
{
    public interface ILeafStrategy
    {
        string Type { get; }
        
        NodeStatus Process(IWorldContext context, IUnit unit);
    }
}