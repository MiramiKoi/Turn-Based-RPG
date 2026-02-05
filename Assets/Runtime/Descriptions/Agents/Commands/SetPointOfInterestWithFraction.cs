using System.Collections.Generic;
using Runtime.Common.Movement;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Extensions;
using Runtime.Units;

namespace Runtime.Descriptions.Agents.Commands
{
    public class SetPointOfInterestWithFraction : CommandDescription
    {
        private const string FractionKey = "fraction";
        private const string UseVisibilityRadiusKey = "use_visibility_radius";
        private const string CustomVisibilityRadiusKey = "custom_visibility_radius";
        private const string TargetPointOfInterestKey = "target";
        
        public string Fraction { get; private set; }
        
        public bool UseVisibilityRadius { get; private set; }
        
        public int CustomVisibilityRadius { get; private set; }
        
        public override string Type => "set_unit_with_fraction";
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
                    
                    break;
                }
            }

            if (targetUnit == null)
            {
                return NodeStatus.Failure;
            }
            
            GridPathfinder.FindPath(context.GridModel, controllableUnit.Position.Value, targetUnit.Position.Value, out var path);

            if (path != null || path.Count < 2)
            {
                controllable.SetPointOfInterest(TargetPointOfInterestKey, path[^1]);
                
                return NodeStatus.Success;
            }
            
            return NodeStatus.Failure;
        }

        public override Dictionary<string, object> Serialize()
        {
            var dictionary = new Dictionary<string, object>();
            
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