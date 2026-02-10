using System.Collections.Generic;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Extensions;
using UnityEngine;

namespace Runtime.Descriptions.Agents.Commands
{
    public class SetRandomPointOfInterestCommand : CommandDescription
    {
        private const string PointOfInterestKey = "point_of_interest";

        private const string RadiusKey = "radius";

        private const string NearWithPointOfInterestKey = "near_with_point_of_interest";

        private const string TargetNearPointOfInterestKey = "target_near_point_of_interest";

        public string PointOfInterest { get; private set; } = string.Empty;

        public int Radius { get; private set; }

        public bool NearWithPointOfInterest { get; private set; }

        public string TargetNearPointOfInterest { get; private set; } = string.Empty;

        public override string Type => "set_random_point_of_interest";

        public override NodeStatus Execute(IWorldContext context, IControllable controllable)
        {
            var center = NearWithPointOfInterest
                ? controllable.GetPointOfInterest(TargetNearPointOfInterest)
                : controllable.Position.Value;

            Vector2Int pointOfInterest;
            Vector2 offset;
            var tryCounter = 3;
            do
            {
                offset = Random.insideUnitCircle * Radius;
                var randomOffset = new Vector2Int(Mathf.RoundToInt(offset.x), Mathf.RoundToInt(offset.y));
                pointOfInterest = center + randomOffset;
                tryCounter--;
            } while (offset.sqrMagnitude > Radius * Radius && context.GridModel.CanPlace(pointOfInterest) &&
                     tryCounter <= 0);

            if (tryCounter <= 0)
            {
                return NodeStatus.Failure;
            }

            controllable.SetPointOfInterest(PointOfInterest, pointOfInterest);

            return NodeStatus.Success;
        }

        public override Dictionary<string, object> Serialize()
        {
            var dictionary = base.Serialize();
            dictionary[PointOfInterestKey] = PointOfInterest;
            dictionary[RadiusKey] = Radius;
            dictionary[NearWithPointOfInterestKey] = NearWithPointOfInterest;
            dictionary[TargetNearPointOfInterestKey] = TargetNearPointOfInterest;
            return dictionary;
        }

        public override void Deserialize(Dictionary<string, object> data)
        {
            PointOfInterest = data.GetString(PointOfInterestKey);
            Radius = data.GetInt(RadiusKey);
            NearWithPointOfInterest = data.GetBool(NearWithPointOfInterestKey);
            TargetNearPointOfInterest = data.GetString(TargetNearPointOfInterestKey);
        }
    }
}