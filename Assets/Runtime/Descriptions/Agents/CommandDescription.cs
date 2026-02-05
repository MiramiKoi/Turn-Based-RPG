using System;
using System.Collections.Generic;
using Runtime.Agents.Nodes;
using Runtime.Extensions;
using Runtime.ModelCollections;

namespace Runtime.Descriptions.Agents
{
    public abstract class CommandDescription : IUnitCommand, ISerializable, IDeserializable
    {
        private const string TypeKey = "type";
        private const string NameKey = "name";
        
        public string Name { get; private set; }
        protected abstract string Type { get; }

        protected CommandDescription(string name)
        {
            Name = name;
        }

        public abstract NodeStatus Execute(IWorldContext context, IControllable controllable);

        public virtual Dictionary<string, object> Serialize()
        {
            return new Dictionary<string, object>()
            {
                {TypeKey, Type},
                {NameKey, Name}
            };
        }

        public virtual void Deserialize(Dictionary<string, object> data)
        {
            Name = data.GetString(NameKey);
        }

        public static CommandDescription CreateCommandFromData(Dictionary<string, object> data)
        {
            var type = data.GetString(TypeKey);
            var name = data.GetString(NameKey);

            CommandDescription command = type switch
            {
                "log" => new LogCommand(name),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            command.Deserialize(data);
            
            return command;
        }
    }
}