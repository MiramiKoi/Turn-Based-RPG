using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Agents;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Descriptions.StatusEffects.Enums;
using Runtime.Extensions;
using Runtime.Units;

namespace Runtime.Descriptions.Agents.Commands
{
    public class ApplyRandomEffectPointOfInterest : CommandDescription
    {
        private const string PointOfInterestKey = "point_of_interest";
        
        public override string Type => "apply_random_effect_point_of_interest";

        public string PointOfInterest { get; private set; } = string.Empty;
        
        public override NodeStatus Execute(IWorldContext context, IControllable controllable)
        {
            var controllableAgent = (AgentModel)controllable;

            if (controllableAgent.PointOfInterest.TryGetValue(PointOfInterest, out var point))
            {
                var cell = context.GridModel.GetCell(point);

                if (cell.Unit is UnitModel unit)
                {
                    var effectsDictionary = context.WorldDescription.StatusEffectCollection.Effects.Where(effect => effect.Value.Polarity == Polarity.Debuff).ToList();

                    var randomEffect = GetRandomValue(effectsDictionary);
                    
                    unit.Effects.TryApply(randomEffect.Key);
                    
                    return NodeStatus.Success;
                }
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
        
        public TValue GetRandomValue<TValue>(List<TValue> list)
        {
            var random = new Random();
            
            if (list == null || list.Count == 0)
                throw new InvalidOperationException("Dictionary is null or empty.");

            return list.ElementAt(random.Next(list.Count));
        }
    }
}