using System.Collections.Generic;
using Runtime.Extensions;

namespace Runtime.Descriptions.Agents.Nodes
{
    public class AgentLeaf : AgentNode
    {
        public override string Type => "leaf";

        public string CommandKey => "command";
        
        public CommandDescription CommandDescription { get; set; }
        
        public override NodeStatus Process(IWorldContext context, IControllable controllable)
        {
            return CommandDescription.Execute(context, controllable);
        }

        public override Dictionary<string, object> Serialize()
        {
            var dictionary = base.Serialize();
            
            dictionary[CommandKey] = CommandDescription.Serialize();
            
            return dictionary;
        }

        public override void Deserialize(Dictionary<string, object> dictionary)
        {
            base.Deserialize(dictionary);
            CommandDescription = CommandDescription.CreateCommandFromData(dictionary.GetNode(CommandKey));
        }
    }
}