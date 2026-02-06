using System.Collections.Generic;
using Runtime.Core;
using Runtime.Extensions;
using Runtime.Units;

namespace Runtime.Descriptions.StatusEffects.Modifiers
{
    public class DisableActionsModifierDescription : ModifierDescription
    {
        private readonly UnitActionType _action;
        
        public DisableActionsModifierDescription(Dictionary<string, object> data) : base(data)
        {
            _action = data.GetEnum<UnitActionType>("action_type");
        }
        
        public override void Tick(UnitModel unit, World world)
        {
            unit.SetActionDisabled(_action, true);
        }
    }
}