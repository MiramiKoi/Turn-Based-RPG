using System.Collections.Generic;
using Runtime.Agents.Nodes;
using Runtime.Extensions;
using UnityEngine;

namespace Runtime.Descriptions.Agents
{
    public class LogCommand : CommandDescription
    {
        private const string MessageKey = "message";

        protected override string Type => "log";

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
            base.Deserialize(dictionary);
            Message = dictionary.GetString(MessageKey);
        }
    }
}