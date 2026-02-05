using System.Collections.Generic;
using Runtime.Descriptions.StatusEffects.Constraints;
using Runtime.Descriptions.StatusEffects.Enums;
using Runtime.Descriptions.StatusEffects.Modifiers;
using Runtime.Extensions;

namespace Runtime.Descriptions.StatusEffects
{
    public class StatusEffectDescription : Description
    {
        private const string CategoryKey = "category";
        private const string PolarityKey = "polarity";
        private const string StackingKey = "stacking";
        private const string DurationKey = "duration";
        private const string ModifiersKey = "modifiers";
        private const string ConstraintsKey = "constraints";
        private const string FlagsKey = "flags";

        public StatusCategory Category { get; }
        public Polarity Polarity { get; }

        public StackingDescription Stacking { get; }
        public DurationDescription Duration { get; }

        public List<ModifierDescription> Modifiers { get; } = new();
        public List<ConstraintDescription> Constraint { get; } = new();

        public StatusFlag Flags { get; }

        public StatusEffectDescription(string id, Dictionary<string, object> data) : base(id)
        {
            Category = data.GetEnum<StatusCategory>(CategoryKey);
            Polarity = data.GetEnum<Polarity>(PolarityKey);

            Stacking = new StackingDescription(data.GetNode(StackingKey));
            Duration = new DurationDescription(data.GetNode(DurationKey));

            var modifierData = data.GetNode(ModifiersKey);

            foreach (var modifier in modifierData)
            {
                var modifierDictionary = (Dictionary<string, object>)modifier.Value;
                
                Modifiers.Add(modifierDictionary.ToModifierDescription(modifier.Key));
            }

            var constraintData = data.GetNode(ConstraintsKey);
            
            foreach (var constraint in constraintData)
            {
                var constraintDictionary = (Dictionary<string, object>)constraint.Value;
                
                Constraint.Add(constraintDictionary.ToConstraintDescription(constraint.Key));
            }
            
            Flags = data.GetFlags<StatusFlag>(FlagsKey);
        }
    }
}