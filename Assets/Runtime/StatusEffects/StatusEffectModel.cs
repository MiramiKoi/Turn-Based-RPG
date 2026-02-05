using System;
using System.Collections.Generic;
using Runtime.Descriptions.StatusEffects;
using Runtime.Descriptions.StatusEffects.Enums;
using Runtime.Extensions;
using Runtime.ModelCollections;

namespace Runtime.StatusEffects
{
    public class StatusEffectModel : ISerializable, IDeserializable
    {
        public string Id { get; }
        public StatusEffectDescription Description { get; }
        public int CurrentStacks { get; private set; }
        public int RemainingTurns { get; private set; }

        public StatusEffectModel(string id, StatusEffectDescription description)
        {
            Id = id;
            Description = description;
            CurrentStacks = 1;
            RemainingTurns = description.Duration.Turns;
        }

        public void AddStack()
        {
            switch (Description.Stacking.Mode)
            {
                case StackingMode.Additive:
                    CurrentStacks = Math.Min(CurrentStacks + 1, Description.Stacking.MaxStacks);
                    break;
                case StackingMode.Refresh:
                    RemainingTurns = Description.Duration.Turns;
                    break;
                case StackingMode.Independent:
                case StackingMode.None:
                    break;
            }
        }

        public void DecrementRemainingTurns()
        {
            RemainingTurns--;
        }

        public bool IsExpired => RemainingTurns <= 0;
        
        public Dictionary<string, object> Serialize()
        {
            return new Dictionary<string, object>
            {
                { "id", Description.Id },
                { "stacks", CurrentStacks },
                { "remaining_turns", RemainingTurns }
            };
        }

        public void Deserialize(Dictionary<string, object> data)
        {
            CurrentStacks = data.GetInt("stacks");
            RemainingTurns = data.GetInt("remaining_turns");
        }
    }
}
