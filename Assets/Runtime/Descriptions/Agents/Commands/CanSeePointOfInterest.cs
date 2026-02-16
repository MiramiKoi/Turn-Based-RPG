using System.Collections.Generic;
using Runtime.Agents;
using Runtime.Common.Vision;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Extensions;
using UnityEngine;

namespace Runtime.Descriptions.Agents.Commands
{
    public class CanSeePointOfInterest : CommandDescription
    {
        private const string PointOfInterestKey = "point_of_interest";
        
        public override string Type => "can_see_point_of_interest";

        public string PointOfInterest { get; private set; } = string.Empty;
        
        public override NodeStatus Execute(IWorldContext context, IControllable controllable)
        {
            var controllableUnit =  controllable as AgentModel;
            
            if (controllableUnit.PointOfInterest.TryGetValue(PointOfInterest, out var position))
            {
                var radius = Mathf.RoundToInt(controllableUnit.Stats["visibility_radius"].Value);
                
                return VisionPathFinder.Trace(context.GridModel, controllableUnit.State.Position.Value, position, radius) ? NodeStatus.Success : NodeStatus.Failure;
            }

            return NodeStatus.Failure;
        }

        public override Dictionary<string, object> Serialize()
        {
            var dictionary =  base.Serialize();
            
            dictionary[PointOfInterestKey] =  PointOfInterest;
            
            return dictionary;
        }

        public override void Deserialize(Dictionary<string, object> data)
        {
            PointOfInterest = data.GetString(PointOfInterestKey);
        }
    }
}