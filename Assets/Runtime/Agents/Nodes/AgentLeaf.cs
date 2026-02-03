using System.Collections.Generic;
using Runtime.Extensions;

namespace Runtime.Agents.Nodes
{
    public class AgentLeaf : AgentNode
    {
        public override string Type => "leaf";

        public string CommandKey => "command";
        
        public string Command { get; set; }
        
        public override NodeStatus Process(IWorldContext context, IControllable controllable)
        {
            return controllable.Commands[Command].Execute(context, controllable);
        }

        public override Dictionary<string, object> Serialize()
        {
            var dictionary = base.Serialize();
            
            dictionary[CommandKey] = Command;
            
            return dictionary;
        }

        public override void Deserialize(Dictionary<string, object> dictionary)
        {
            base.Deserialize(dictionary);
            Command = dictionary.GetString(CommandKey);
        }
    }
}