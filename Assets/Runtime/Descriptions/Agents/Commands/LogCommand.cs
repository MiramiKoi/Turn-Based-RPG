using System.Collections.Generic;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Extensions;

namespace Runtime.Descriptions.Agents.Commands
{
    public class LogCommand : CommandDescription
    {
        private const string MessageKey = "message";

        public override string Type => "log";

        public string Message { get; private set; } = string.Empty;

        public override NodeStatus Execute(IWorldContext context, IControllable controllable)
        {
            return NodeStatus.Success;
        }

        public override Dictionary<string, object> Serialize()
        {
            var dictionary = base.Serialize();
            
            dictionary[MessageKey] = Message;
            
            return dictionary;
        }

        public override void Deserialize(Dictionary<string, object> dictionary)
        {
            Message = dictionary.GetString(MessageKey);
        }
    }
}