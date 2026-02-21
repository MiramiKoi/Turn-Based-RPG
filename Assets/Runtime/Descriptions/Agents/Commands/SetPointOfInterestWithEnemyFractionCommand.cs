using System.Collections.Generic;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Extensions;
using Runtime.Units;

namespace Runtime.Descriptions.Agents.Commands
{
    public class SetPointOfInterestWithEnemyFractionCommand : CommandDescription
    {
        private const string UseVisibilityRadiusKey = "use_visibility_radius";
        private const string CustomVisibilityRadiusKey = "custom_visibility_radius";
        private const string PointOfInterestKey = "point_of_interest";

        public override string Type => "set_point_of_interest_with_enemy_fraction";

        public bool UseVisibilityRadius { get; private set; }

        public int CustomVisibilityRadius { get; private set; }

        public string PointOfInterest { get; private set; } = string.Empty;

        public override NodeStatus Execute(IWorldContext context, IControllable controllable)
        {
            var radius = UseVisibilityRadius
                ? controllable.Stats["visibility_radius"].Value
                : CustomVisibilityRadius;

            var center = controllable.State.Position.Value;
            var controllableUnit = controllable as UnitModel;

            if (controllableUnit == null)
                return NodeStatus.Failure;

            UnitModel targetUnit = null;
            var enemyFractions = controllableUnit.Description.EnemyFractions;

            foreach (var unit in context.UnitCollection.Models.Values)
            {
                if (unit.Id == controllableUnit.Id)
                    continue;

                if (!enemyFractions.Contains(unit.Description.Fraction))
                    continue;

                var dx = unit.State.Position.Value.x - center.x;
                var dy = unit.State.Position.Value.y - center.y;

                var distanceSquared = dx * dx + dy * dy;

                if (distanceSquared <= radius * radius)
                {
                    targetUnit = unit;
                }
            }

            if (targetUnit == null)
                return NodeStatus.Failure;

            controllable.SetPointOfInterest(PointOfInterest, targetUnit.State.Position.Value);

            return NodeStatus.Success;
        }

        public override Dictionary<string, object> Serialize()
        {
            var dictionary = base.Serialize();
            dictionary[UseVisibilityRadiusKey] = UseVisibilityRadius;
            dictionary[CustomVisibilityRadiusKey] = CustomVisibilityRadius;
            dictionary[PointOfInterestKey] = PointOfInterest;
            return dictionary;
        }

        public override void Deserialize(Dictionary<string, object> data)
        {
            UseVisibilityRadius = data.GetBool(UseVisibilityRadiusKey);
            CustomVisibilityRadius = data.GetInt(CustomVisibilityRadiusKey);
            PointOfInterest = data.GetString(PointOfInterestKey);
        }
    }
}