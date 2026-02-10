using System.Collections.Generic;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Extensions;
using Runtime.Units;

namespace Runtime.Descriptions.Agents.Commands
{
    public class HasUnitWithFractionCommand : CommandDescription
    {
        private const string FractionKey = "fraction";
        private const string UseVisibilityRadiusKey = "use_visibility_radius";
        private const string CustomVisibilityRadiusKey = "custom_visibility_radius";

        public override string Type => "has_unit_with_fraction";

        public string Fraction { get; private set; } = string.Empty;

        public bool UseVisibilityRadius { get; private set; }

        public int CustomVisibilityRadius { get; private set; }

        public override NodeStatus Execute(IWorldContext context, IControllable controllable)
        {
            var radius = UseVisibilityRadius ? controllable.Stats["visibility_radius"].Value : CustomVisibilityRadius;

            var center = controllable.Position.Value;

            var controllableUnit = controllable as UnitModel;

            foreach (var unit in context.UnitCollection.Models.Values)
            {
                if (unit.Description.Fraction != Fraction || unit.Id == controllableUnit?.Id)
                {
                    continue;
                }

                var dx = unit.Position.Value.x - center.x;
                var dy = unit.Position.Value.y - center.y;

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

            dictionary[FractionKey] = Fraction;
            dictionary[UseVisibilityRadiusKey] = UseVisibilityRadius;
            dictionary[CustomVisibilityRadiusKey] = CustomVisibilityRadius;

            return dictionary;
        }

        public override void Deserialize(Dictionary<string, object> data)
        {
            Fraction = data.GetString(FractionKey);
            UseVisibilityRadius = data.GetBool(UseVisibilityRadiusKey);
            CustomVisibilityRadius = data.GetInt(CustomVisibilityRadiusKey);
        }
    }
}