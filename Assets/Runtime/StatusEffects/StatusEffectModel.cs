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
        public bool IsExpired
        {
            get => _isExpired || (RemainingTurns.Value <= 0 && Description.Duration.Type == DurationType.TurnBased);
            set => _isExpired = value;
        }

        public StatusEffectDescription Description { get; }
        public ReactiveProperty<int> CurrentStacks { get; } = new();
        public ReactiveProperty<int> RemainingTurns { get; } = new();
        
        private bool _isExpired;

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
                {
                    var before = CurrentStacks.Value;
                    CurrentStacks.Value = Math.Min(CurrentStacks.Value + 1, Description.Stacking.MaxStacks);

                    if (CurrentStacks.Value == Description.Stacking.MaxStacks &&
                        before == Description.Stacking.MaxStacks)
                        RemainingTurns.Value = Description.Duration.Turns;

                    break;
                }
                case StackingMode.Refresh:
                    RemainingTurns.Value = Description.Duration.Turns;
                    CurrentStacks.Value = 1;
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

        public Dictionary<string, object> Serialize()
        {
            return new Dictionary<string, object>
            {
                { "id", Description.Id },
                { "stacks", CurrentStacks.Value },
                { "remaining_turns", RemainingTurns.Value }
            };
        }

        public void Deserialize(Dictionary<string, object> data)
        {
            CurrentStacks.Value = data.GetInt("stacks");
            RemainingTurns.Value = data.GetInt("remaining_turns");
        }
    }
}