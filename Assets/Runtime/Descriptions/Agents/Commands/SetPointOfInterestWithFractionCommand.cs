using System.Collections.Generic;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Extensions;
using Runtime.Units;

namespace Runtime.Descriptions.Agents.Commands
{
    public class SetPointOfInterestWithFractionCommand : CommandDescription
    {
        private const string FractionKey = "fraction";
        private const string UseVisibilityRadiusKey = "use_visibility_radius";
        private const string CustomVisibilityRadiusKey = "custom_visibility_radius";
        private const string PointOfInterestKey = "point_of_interest";
        
        public string Fraction { get; private set; } = string.Empty;
        
        public bool UseVisibilityRadius { get; private set; }
        
        public int CustomVisibilityRadius { get; private set; }

        public string PointOfInterest { get; private set; } = string.Empty;
        
        public override string Type => "set_point_of_interest_with_fraction";
        public override NodeStatus Execute(IWorldContext context, IControllable controllable)
        {
            var radius = UseVisibilityRadius ? controllable.Stats["visibility_radius"].Value : CustomVisibilityRadius;

            var center = controllable.Position.Value;
            
            var controllableUnit = controllable as UnitModel;

            UnitModel targetUnit = null;
            
            foreach (var unit in context.UnitCollection.Models.Values)
            {
                if (unit.Description.Fraction != Fraction || unit.Id == controllableUnit?.Id)
                {
                    continue;
                }
                
                var dx = unit.Position.Value.x - center.x;
                var dy =  unit.Position.Value.y - center.y;
                
                var distanceSquared = dx * dx + dy * dy;

                if (distanceSquared <= radius * radius)
                {
                    targetUnit = unit;
                }
            }

            if (targetUnit == null)
            {
                return NodeStatus.Failure;
            }
            
            controllable.SetPointOfInterest(PointOfInterest, targetUnit.Position.Value);
            
            return NodeStatus.Success;
        }

        public override Dictionary<string, object> Serialize()
        {
            var dictionary = base.Serialize();
            
            dictionary[FractionKey] = Fraction;
            dictionary[UseVisibilityRadiusKey] = UseVisibilityRadius;
            dictionary[CustomVisibilityRadiusKey] = CustomVisibilityRadius;
            dictionary[PointOfInterestKey] = PointOfInterest;
            
            return dictionary;
        }

        public override void Deserialize(Dictionary<string, object> data)
        {
            Fraction = data.GetString(FractionKey);
            UseVisibilityRadius = data.GetBool(UseVisibilityRadiusKey);
            CustomVisibilityRadius = data.GetInt(CustomVisibilityRadiusKey);
            PointOfInterest = data.GetString(PointOfInterestKey);
        }
    }
}