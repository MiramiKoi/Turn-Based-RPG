namespace Runtime.Agents.Nodes
{
    public interface IUnitCommand
    {
        NodeStatus Execute(IWorldContext context, IControllable controllable);
    }
}