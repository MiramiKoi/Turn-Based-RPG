using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Agents;
using Runtime.Descriptions.Agents.Nodes;
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
            var controllableAgent = controllable as AgentModel;

            if (controllableAgent.PointOfInterest.TryGetValue(PointOfInterest, out var point))
            {
                var cell = context.GridModel.GetCell(point);

                if (cell.Unit is UnitModel unit)
                {
                    var effectsDictionary = context.WorldDescription.StatusEffectCollection.Effects;

                    var randomEffect = GetRandomValue(effectsDictionary);
                    
                    unit.Effects.TryApply(randomEffect.LuaScript);
                    
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
        
        public TValue GetRandomValue<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            var random = new Random();
            
            if (dictionary == null || dictionary.Count == 0)
                throw new InvalidOperationException("Dictionary is null or empty.");

            return dictionary.Values.ElementAt(random.Next(dictionary.Count));
        }
    }
}