using System.Collections.Generic;
using Runtime.Core;
using Runtime.Descriptions.StatusEffects.Enums;
using Runtime.Extensions;
using Runtime.Units;

namespace Runtime.Descriptions.StatusEffects.Modifiers
{
    public class ModifyStatModifierDescription : ModifierDescription
    {
        private int Value { get; }
        private string Stat { get; }
        private StatModifyMode Mode { get; }

        public ModifyStatModifierDescription(Dictionary<string, object> data) : base(data)
        {
            Value = data.GetInt("amount");
            Stat = data.GetString("stat");
            Mode = data.GetEnum<StatModifyMode>("mode");
        }
        
        public override void Tick(UnitModel unit, World world)
        {
            var statModel = unit.Stats[Stat];

            switch (Mode)
            {
                case StatModifyMode.Add:
                    statModel.ChangeValue(Value);
                    break;

                case StatModifyMode.Multiply:
                    statModel.MultiplyValue(Value);
                    break;

                case StatModifyMode.AddPercent:
                {
                    var delta = statModel.Value * (Value / 100f);
                    statModel.ChangeValue(delta);
                    break;
                }

                case StatModifyMode.MultiplyPercent:
                {
                    var factor = 1f + Value / 100f;
                    statModel.MultiplyValue(factor);
                    break;
                }

                case StatModifyMode.Set:
                    statModel.SetValue(Value);
                    break;
            }
        }
        
        public override void Tick(UnitModel unit, World world, int stacks)
        {
            var k = stacks <= 0 ? 1 : stacks;

            if (Mode is StatModifyMode.Add or StatModifyMode.AddPercent)
            {
                var scaled = Value * k;

                var statModel = unit.Stats[Stat];

                switch (Mode)
                {
                    case StatModifyMode.Add:
                        statModel.ChangeValue(scaled);
                        return;

                    case StatModifyMode.AddPercent:
                    {
                        var delta = statModel.Value * (scaled / 100f);
                        statModel.ChangeValue(delta);
                        return;
                    }
                }
            }

            Tick(unit, world);
        }
    }
}