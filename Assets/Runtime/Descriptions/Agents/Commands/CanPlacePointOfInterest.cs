using System.Collections.Generic;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Extensions;
using UnityEngine;

namespace Runtime.Descriptions.Agents.Commands
{
    public class CanPlacePointOfInterest : CommandDescription
    {
        public override string Type => "can_place_point_of_interest";
        
        private const string PointOfInterestKey = "point_of_interest";

        public string PointOfInterest { get; private set; } = string.Empty; 
        
        public override NodeStatus Execute(IWorldContext context, IControllable controllable)
        {
            if (!controllable.PointOfInterest.ContainsKey(PointOfInterest))
            {
                return NodeStatus.Failure;
            }
            
            var canPlace = context.GridModel.CanPlace(controllable.GetPointOfInterest(PointOfInterest));
            
            return canPlace ? NodeStatus.Success  : NodeStatus.Failure;;
        }

        public override Dictionary<string, object> Serialize()
        {
            var dictionary = base.Serialize();
            
            dictionary[PointOfInterestKey] = PointOfInterest;
            
            return dictionary;
        }

        public override void Deserialize(Dictionary<string, object> data)
        {
            PointOfInterest = data.GetString(PointOfInterestKey);
        }
    }
}