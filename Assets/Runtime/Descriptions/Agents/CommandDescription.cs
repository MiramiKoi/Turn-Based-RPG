using System;
using System.Collections.Generic;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Extensions;
using Runtime.ModelCollections;

namespace Runtime.Descriptions.Agents
{
    public abstract class CommandDescription : IUnitCommand, ISerializable, IDeserializable
    {
        public const string TypeKey = "type";
        private const string NameKey = "name";
        
        protected abstract string Type { get; }
        
        public abstract NodeStatus Execute(IWorldContext context, IControllable controllable);

        public virtual Dictionary<string, object> Serialize()
        {
            return new Dictionary<string, object>()
            {
                {TypeKey, Type},
            };
        }

        public virtual void Deserialize(Dictionary<string, object> data)
        {
        }

        public static CommandDescription CreateCommand(string type)
        {
            CommandDescription command = type switch
            {
                "log" => new LogCommand(),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            return command;
        }
        
        public static CommandDescription CreateCommandFromData(Dictionary<string, object> data)
        {
            var type = data.GetString(TypeKey);
            
            var command = CreateCommand(type);
            
            command.Deserialize(data);
            
            return command;
        }
    }
}