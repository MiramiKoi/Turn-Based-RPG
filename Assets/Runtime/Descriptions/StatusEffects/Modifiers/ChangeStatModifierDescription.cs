using System.Collections.Generic;
using Runtime.Core;
using Runtime.Extensions;
using Runtime.Units;

namespace Runtime.Descriptions.StatusEffects.Modifiers
{
    public class ChangeStatModifierDescription : ModifierDescription
    {
        private int Amount { get; }
        private string Stat { get; }

        public ChangeStatModifierDescription(Dictionary<string, object> data) : base(data)
        {
            Amount = data.GetInt("amount");
            Stat = data.GetString("stat");
        }
        
        public override void Tick(UnitModel unit, World world)
        {
            unit.Stats[Stat].ChangeValue(Amount);
        }
        
        public override void Tick(UnitModel unit, World world, int stacks)
        {
            var coefficient = stacks <= 0 ? 1 : stacks;
            unit.Stats[Stat].ChangeValue(Amount * coefficient);
        }
    }
}