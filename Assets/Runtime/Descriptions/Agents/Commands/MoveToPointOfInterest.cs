using System.Collections.Generic;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Extensions;
using Runtime.Units;
using UnityEngine;

namespace Runtime.Descriptions.Agents.Commands
{
    public class MoveToPointOfInterest : CommandDescription
    {
        private const string PointOfInterestKey = "point_of_interest";
        public override string Type => "move_to_point_of_interest";

        public string PointOfInterest { get; private set; } = string.Empty;
        
        public override NodeStatus Execute(IWorldContext context, IControllable controllable)
        {
            var unit = (UnitModel)controllable;
            
            var pointOfInterest = unit.GetPointOfInterest(PointOfInterest);
            
            var targetPosition = GetPosition(context, unit, pointOfInterest);
            
            if (unit.IsDead)
            {
                return NodeStatus.Failure;
            }
            
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
            var dictionary = base.Serialize();
            dictionary[PointOfInterestKey] = PointOfInterest;
            return dictionary;
        }

        public override void Deserialize(Dictionary<string, object> data)
        {
            PointOfInterest = data.GetString(PointOfInterestKey);
        }

        private Vector2Int GetPosition(IWorldContext context, UnitModel unit, Vector2Int endPosition)
        {
            var currentPosition = unit.Position.Value;
            var bestPosition = currentPosition;
            var currentDistance = Vector2Int.Distance(currentPosition, endPosition);

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    var candidate = currentPosition + new Vector2Int(x, y);

                    if (!context.GridModel.CanPlace(candidate) && context.GridModel.GetCell(candidate).Unit != unit)
                        continue;

                    var candidateDistance = Vector2Int.Distance(candidate, endPosition);

                    if (candidateDistance < currentDistance)
                    {
                        currentDistance = candidateDistance;
                        bestPosition = candidate;
                    }
                }
            }

            return bestPosition;
        }
    }
}