using Runtime.Core;
using Runtime.Units;

namespace Runtime.Descriptions.StatusEffects.Modifiers
{
    public abstract class ModifierDescription
    {
        public abstract void Tick(UnitModel unit, World world);
    }
}