using System;
using System.Collections.Generic;
using Runtime.Descriptions.StatusEffects;
using Runtime.Descriptions.StatusEffects.Enums;
using Runtime.Extensions;
using Runtime.ModelCollections;
using UniRx;

namespace Runtime.StatusEffects
{
    public class StatusEffectModel : ISerializable, IDeserializable
    {
        public string Id { get; }
        public StatusEffectDescription Description { get; }
        public ReactiveProperty<int> CurrentStacks { get; private set; } = new();
        public ReactiveProperty<int> RemainingTurns { get; private set; } = new();

        public StatusEffectModel(string id, StatusEffectDescription description)
        {
            Id = id;
            Description = description;
            CurrentStacks.Value = 1;
            RemainingTurns.Value = description.Duration.Turns;
        }

        public void AddStack()
        {
            switch (Description.Stacking.Mode)
            {
                case StackingMode.Additive:
                    CurrentStacks.Value = Math.Min(CurrentStacks.Value + 1, Description.Stacking.MaxStacks);
                    break;
                case StackingMode.Refresh:
                    RemainingTurns.Value = Description.Duration.Turns;
                    break;
                case StackingMode.Independent:
                case StackingMode.None:
                    break;
            }
        }

        public void DecrementRemainingTurns()
        {
            RemainingTurns.Value--;
        }

        public bool IsExpired => RemainingTurns.Value <= 0 && Description.Duration.Type == DurationType.TurnBased;

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
            CurrentStacks.Value = data.GetInt("stacks");
            RemainingTurns.Value = data.GetInt("remaining_turns");
        }
    }
}