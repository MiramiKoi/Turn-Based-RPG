using System.Collections.Generic;
using Runtime.Extensions;
using Runtime.ModelCollections;

namespace Runtime.Descriptions.Agents
{
    public class CommandDescriptionCollection : ISerializable, IDeserializable
    {
        private readonly Dictionary<string, CommandDescription> _commands = new();

        public CommandDescription Get(CommandDescription command)
        {
            return _commands[command.Name];
        }

        public void Register(CommandDescription command)
        {
            _commands[command.Name] = command;
        }

        public void Unregister(CommandDescription command)
        {
            _commands.Remove(command.Name);   
        }

        public Dictionary<string, object> Serialize()
        {
            var dictionary = new Dictionary<string, object>();

            foreach (var command in _commands.Values)
            {
                dictionary[command.Name] = command.Serialize();
            }

            return dictionary;
        }

        public void Deserialize(Dictionary<string, object> data)
        {
            foreach (var pair in data)
            {
                var command = CommandDescription.CreateCommandFromData(data.GetNode(pair.Key));
                
                Register(command);
            }
        }
    }
}