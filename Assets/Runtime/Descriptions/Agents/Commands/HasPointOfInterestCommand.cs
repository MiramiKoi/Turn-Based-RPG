using System.Collections.Generic;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Extensions;

namespace Runtime.Descriptions.Agents.Commands
{
    public class HasPointOfInterestCommand : CommandDescription
    {
        private const string PointOfInterestKey = "point_of_interest";

        public override string Type => "has_point_of_interest";

        public string PointOfInterest { get; private set; } = string.Empty;
        
        public override NodeStatus Execute(IWorldContext context, IControllable controllable)
        {
            var hasPointOfInterest = controllable.PointOfInterest.ContainsKey(PointOfInterestKey);
            
            return hasPointOfInterest ? NodeStatus.Success : NodeStatus.Failure;
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