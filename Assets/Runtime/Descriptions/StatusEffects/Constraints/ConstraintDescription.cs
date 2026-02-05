using Runtime.Core;
using Runtime.Units;

namespace Runtime.Descriptions.StatusEffects.Constraints
{
    public abstract class ConstraintDescription
    {
        public abstract bool Check(UnitModel unitModel, World world);
    }
}