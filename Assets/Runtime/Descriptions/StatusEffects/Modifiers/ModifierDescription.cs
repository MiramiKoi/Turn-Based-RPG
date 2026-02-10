using System.Collections.Generic;
using Runtime.Core;
using Runtime.Descriptions.StatusEffects.Enums;
using Runtime.Extensions;
using Runtime.Units;

namespace Runtime.Descriptions.StatusEffects.Modifiers
{
    public abstract class ModifierDescription
    {
        public ModifierExecutionTime Type { get; }

        protected ModifierDescription(Dictionary<string, object> data)
        {
            Type = data.GetEnum<ModifierExecutionTime>("type");
        }

        public abstract void Tick(UnitModel unit, World world);

        public virtual void Tick(UnitModel unit, World world, int stacks)
        {
            Tick(unit, world);
        }
    }
}