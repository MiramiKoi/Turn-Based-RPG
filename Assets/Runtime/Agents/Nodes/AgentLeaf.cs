using System.Collections.Generic;

namespace Runtime.Agents.Nodes
{
    public class AgentLeaf : AgentNode
    {
        private const string StrategyKey = "strategy";

        public override string Type => "leaf";

        public string CommandKey => "command";
        
        public string Command { get; set; }
        
        protected ILeafStrategy _strategy; 
        
        public override NodeStatus Process(IWorldContext context, IUnit unit)
        {
            return _strategy.Process(context, unit);   
        }

        public override Dictionary<string, object> Serialize()
        {
            var dictionary = base.Serialize();
            
            dictionary[StrategyKey] = _strategy.Type;
            dictionary[CommandKey] = Command;
            
            return dictionary;
        }

        public override void Deserialize(Dictionary<string, object> data)
        {
            base.Deserialize(data);
        }
    }
}