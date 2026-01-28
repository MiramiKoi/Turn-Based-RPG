using System.Collections.Generic;

namespace Runtime.Agents.Nodes
{
    public interface IUnit
    {
        IReadOnlyDictionary<string, IUnitCommand> AvailableCommands { get; }
    }
}