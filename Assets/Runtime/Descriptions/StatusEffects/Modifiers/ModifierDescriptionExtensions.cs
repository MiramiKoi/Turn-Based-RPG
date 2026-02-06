using System.Collections.Generic;

namespace Runtime.Descriptions.StatusEffects.Modifiers
{
    public static class ModifierDescriptionExtensions
    {
        private const string ModifyStatKey = "modify_stat";
        private const string DisableActionsKey = "disable_actions";
        private const string RandomizeActionKey = "randomize_action";
        
        public static ModifierDescription ToModifierDescription(this Dictionary<string, object> data, string type)
        {
            return type switch
            {
                ModifyStatKey => new ModifyStatModifierDescription(data),
                DisableActionsKey => new DisableActionsModifierDescription(data),
                RandomizeActionKey => new RandomizeActionModifierDescription(data),
                _ =>  null
            };
        }
    }
}