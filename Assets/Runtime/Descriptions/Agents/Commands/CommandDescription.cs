using System;
using System.Collections.Generic;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Extensions;
using Runtime.ModelCollections;

namespace Runtime.Descriptions.Agents.Commands
{
    public abstract class CommandDescription : IUnitCommand, ISerializable, IDeserializable
    {
        public const string TypeKey = "type";
        private const string NameKey = "name";

        public abstract string Type { get; }
        
        public abstract NodeStatus Execute(IWorldContext context, IControllable controllable);

        public virtual Dictionary<string, object> Serialize()
        {
            return new Dictionary<string, object>
            {
                {TypeKey, Type}
            };
        }

        public abstract void Deserialize(Dictionary<string, object> data);

        public static CommandDescription CreateCommand(string type)
        {
            CommandDescription command = type switch
            {
                "log" => new LogCommand(),
                "distance_point_of_interest" => new DistancePointOfInterest(),
                "has_flag" => new HasFlagCommand(),
                "has_point_of_interest" => new HasPointOfInterest(),
                "move_to_point_of_interest" => new MoveToPointOfInterest(),
                "set_flag" => new SetFlagCommand(),
                "set_random_point_of_interest" => new SetRandomPointOfInterest(),
                "has_unit_with_fraction" => new HasUnitWithFraction(),
                "set_point_of_interest_with_fraction" => new SetPointOfInterestWithFraction(),
                "can_place_point_of_interest" => new CanPlacePointOfInterest(),
                "attack_point_of_interest" => new AttackPointOfInterest(),
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