namespace Runtime.Agents.Nodes
{
    public interface IUnit
    {
        public IUnitCommand TryGetCommand(string key);
    }
}