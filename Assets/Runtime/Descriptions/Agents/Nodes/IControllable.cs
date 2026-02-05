using System.Collections.Generic;

namespace Runtime.Agents.Nodes
{
    public interface IControllable
    {
        IReadOnlyDictionary<string, IUnitCommand> Commands { get; }

        IReadOnlyDictionary<string, bool> Flags { get; }
        
        
        public void SetFlag(string key, bool value);
    }
}