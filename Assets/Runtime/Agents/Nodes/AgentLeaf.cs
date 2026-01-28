using System.Collections.Generic;

namespace Runtime.Agents.Nodes
{
    public class AgentLeaf : AgentNode
    {
        private const string StrategyKey = "strategy";

        public override string Type => "leaf";

        public string CommandKey => "command";
        
        public string Command { get; set; }
        
        public override NodeStatus Process(IWorldContext context, IUnit unit)
        {
            var command = unit.TryGetCommand(CommandKey);
            
            return command.Execute(context, unit);
        }

        public override Dictionary<string, object> Serialize()
        {
            var dictionary = base.Serialize();
            
            dictionary[CommandKey] = Command;
            
            return dictionary;
        }
    }
}