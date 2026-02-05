using System.Collections.Generic;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Extensions;
using Runtime.Units;

namespace Runtime.Descriptions.Agents.Commands
{
    public class MoveToPointOfInterest : CommandDescription
    {
        private const string PointOfInterestKey = "point_of_interest";
        public override string Type => "move_to_point_of_interest";

        public string PointOfInterest { get; private set; } = string.Empty;
        
        public override NodeStatus Execute(IWorldContext context, IControllable controllable)
        {
            var targetPosition = controllable.GetPointOfInterest(PointOfInterest);
            var unit = controllable as UnitModel;
            
            if (context.GridModel.TryPlace(unit, targetPosition))
            {
                context.GridModel.ReleaseCell(unit.Position.Value);
                unit.MoveTo(targetPosition);
                return NodeStatus.Success;
            }
            
            return NodeStatus.Failure;
        }

        public override Dictionary<string, object> Serialize()
        {
            var dictionary = new Dictionary<string, object>();
            dictionary[PointOfInterestKey] = PointOfInterest;
            return dictionary;
        }

        public override void Deserialize(Dictionary<string, object> data)
        {
            PointOfInterest = data.GetString(PointOfInterestKey);
        }
    }
}