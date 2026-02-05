using System.Collections.Generic;
using System.Data;
using Runtime.ModelCollections;

namespace Runtime.Descriptions.Agents
{
    public class AgentDescription : ISerializable, IDeserializable
    {
        private const string CommandsKey = "commands";

        public CommandDescriptionCollection Commands { get; } = new();
        
        public Dictionary<string, object> Serialize()
        {
            var dictionary = new Dictionary<string, object>()
            {
                {CommandsKey, Commands.Serialize()}
            };

            return dictionary;
        }

        public void Deserialize(Dictionary<string, object> data)
        {
            Commands.Deserialize(data);
        }
    }
}