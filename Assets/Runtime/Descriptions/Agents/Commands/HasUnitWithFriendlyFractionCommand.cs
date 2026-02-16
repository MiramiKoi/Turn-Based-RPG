using System.Collections.Generic;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Extensions;
using Runtime.Units;

namespace Runtime.Descriptions.Agents.Commands
{
    public class HasUnitWithFriendlyFractionCommand : CommandDescription
    {
        private const string UseVisibilityRadiusKey = "use_visibility_radius";
        private const string CustomVisibilityRadiusKey = "custom_visibility_radius";
        public override string Type => "has_unit_with_friendly_fraction";
        
        public bool UseVisibilityRadius { get; private set; }

        public int CustomVisibilityRadius { get; private set; }
        
        public override NodeStatus Execute(IWorldContext context, IControllable controllable)
        {
            var radius = UseVisibilityRadius
                ? controllable.Stats["visibility_radius"].Value
                : CustomVisibilityRadius;

            var center = controllable.State.Position.Value;
            var controllableUnit = controllable as UnitModel;

            if (controllableUnit == null)
                return NodeStatus.Failure;

            var friendlyFractions = controllableUnit.Description.FriendlyFractions;

            foreach (var unit in context.UnitCollection.Models.Values)
            {
                if (unit.Id == controllableUnit.Id)
                    continue;

                if (!friendlyFractions.Contains(unit.Description.Fraction))
                    continue;

                var dx = unit.State.Position.Value.x - center.x;
                var dy = unit.State.Position.Value.y - center.y;

                var distanceSquared = dx * dx + dy * dy;

                if (distanceSquared <= radius * radius)
                {
                    return NodeStatus.Success;
                }
            }

            return NodeStatus.Failure;
        }

        public override Dictionary<string, object> Serialize()
        {
            var dictionary = base.Serialize();

            dictionary[UseVisibilityRadiusKey] = UseVisibilityRadius;
            dictionary[CustomVisibilityRadiusKey] = CustomVisibilityRadius;

            return dictionary;
        }

        public override void Deserialize(Dictionary<string, object> data)
        {
            UseVisibilityRadius = data.GetBool(UseVisibilityRadiusKey);
            CustomVisibilityRadius = data.GetInt(CustomVisibilityRadiusKey);
        }
    }
}