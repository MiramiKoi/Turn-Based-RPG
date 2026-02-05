using System.Collections.Generic;

namespace Runtime.Descriptions.StatusEffects.Modifiers
{
    public static class ModifierDescriptionExtensions
    {
        private const string ChangeStatKey = "change_stat";
        
        public static ModifierDescription ToModifierDescription(this Dictionary<string, object> data, string type)
        {
            return type switch
            {
                ChangeStatKey => new ChangeStatModifierDescription(data),
                _ =>  null
            };
        }
    }
}