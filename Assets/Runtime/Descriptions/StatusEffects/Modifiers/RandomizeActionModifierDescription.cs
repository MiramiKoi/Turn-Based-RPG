using System.Collections.Generic;
using Runtime.Core;
using Runtime.Units;

namespace Runtime.Descriptions.StatusEffects.Modifiers
{
    public class RandomizeActionModifierDescription : ModifierDescription
    {
        public RandomizeActionModifierDescription(Dictionary<string, object> data) : base(data)
        {
        }

        public override void Tick(UnitModel unit, World world)
        {
        }
    }
}