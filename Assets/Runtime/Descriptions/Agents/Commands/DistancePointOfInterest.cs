using System.Collections.Generic;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Extensions;
using UnityEngine;

namespace Runtime.Descriptions.Agents.Commands
{
    public class DistancePointOfInterest : CommandDescription
    {
        private const string PointOfInterestKey = "point_of_interest";
        private const string DistanceKey = "distance";
        private const string OperationKey = "operation";

        public override string Type => "distance_point_of_interest";

        public string PointOfInterest { get; private set; } = string.Empty;
        
        public int Distance { get; private set; } = 0;
        
        public string Operation { get; private set; } = string.Empty;
        
        public override NodeStatus Execute(IWorldContext context, IControllable controllable)
        {
            var position = controllable.Position.Value;
            var target = controllable.GetPointOfInterest(PointOfInterestKey);
            
            var distance = Mathf.RoundToInt(Vector2Int.Distance(position, target));

            return Operation switch
            {
                "=" => distance == Distance ? NodeStatus.Success : NodeStatus.Failure,
                ">" => distance > Distance ? NodeStatus.Success : NodeStatus.Failure,
                "<" => distance < Distance ? NodeStatus.Success : NodeStatus.Failure,
                "<=" => distance <= Distance ? NodeStatus.Success : NodeStatus.Failure,
                ">=" => distance >= Distance ? NodeStatus.Success : NodeStatus.Failure,
                _ => NodeStatus.Failure
            };
        }

        public override Dictionary<string, object> Serialize()
        {
            var dictionary = base.Serialize();
            dictionary[PointOfInterestKey] = PointOfInterest;
            dictionary[OperationKey] = Operation;
            dictionary[DistanceKey] = Distance;
            return dictionary;
        }

        public override void Deserialize(Dictionary<string, object> dictionary)
        {
            base.Deserialize(dictionary);
            PointOfInterest = dictionary.GetString(PointOfInterestKey);
            Distance = dictionary.GetInt(DistanceKey);
            Operation = dictionary.GetString(OperationKey);
        }
    }
}