using System.Collections.Generic;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Extensions;
using Runtime.Units;
using Runtime.Units.Rotation;

namespace Runtime.Descriptions.Agents.Commands
{
    public class AttackPointOfInterestCommand : CommandDescription
    {
        public override string Type => "attack_point_of_interest";

        private const string PointOfInterestKey = "point_of_interest";

        public string PointOfInterest { get; private set; } = string.Empty;

        public override NodeStatus Execute(IWorldContext context, IControllable controllable)
        {
            var pointOfInterestPosition = controllable.GetPointOfInterest(PointOfInterest);

            var targetCell = context.GridModel.GetCell(pointOfInterestPosition);

            var targetUnit = targetCell.Unit as UnitModel;
            var controllableUnit = (UnitModel)controllable;

            if (targetUnit == null)
            {
                return NodeStatus.Failure;
            }

            if (controllableUnit.State.Position.Value.x != targetCell.Position.x)
                controllableUnit.Movement.Rotate(targetCell.Position.x < controllableUnit.State.Position.Value.x ? UnitDirection.Left : UnitDirection.Right);
            
            targetUnit.Combat.TakeDamage(controllableUnit.Combat.GetDamage());

            return NodeStatus.Success;
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