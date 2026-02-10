using System.Collections.Generic;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Extensions;
using Runtime.Units;

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
            var controllableUnit = controllable as UnitModel;

            if (targetUnit == null)
            {
                return NodeStatus.Failure;
            }
            
            targetUnit.TakeDamage(controllableUnit.GetDamage());

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