using System.Collections.Generic;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Extensions;

namespace Runtime.Descriptions.Agents.Commands
{
    public class CanPlacePointOfInterestCommand : CommandDescription
    {
        public override string Type => "can_place_point_of_interest";

        private const string PointOfInterestKey = "point_of_interest";

        public string PointOfInterest { get; private set; } = string.Empty;

        public override NodeStatus Execute(IWorldContext context, IControllable controllable)
        {
            if (!controllable.PointOfInterest.TryGetValue(PointOfInterest, out var pointOfInterest) ||
                pointOfInterest.x > context.GridModel.Cells.GetLength(0) ||
                pointOfInterest.y > context.GridModel.Cells.GetLength(1) || pointOfInterest.x < 0 || pointOfInterest.y < 0)
            {
                return NodeStatus.Failure;
            }


            var canPlace = context.GridModel.CanPlace(controllable.GetPointOfInterest(PointOfInterest));

            return canPlace ? NodeStatus.Success : NodeStatus.Failure;
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