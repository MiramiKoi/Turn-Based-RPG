using System.Collections.Generic;

namespace Runtime.Descriptions.StatusEffects
{
    public class StatusEffectDescriptionCollection
    {
        public Dictionary<string, StatusEffectDescription> Effects { get; }

        public StatusEffectDescriptionCollection(Dictionary<string, object> data)
        {
            Effects = new Dictionary<string, StatusEffectDescription>();

            foreach (var pair in data)
            {
                var effectData = pair.Value as Dictionary<string, object>;
                var description = new StatusEffectDescription(pair.Key, effectData);

                Effects.Add(pair.Key, description);
            }
        }

        public StatusEffectDescription Get(string id)
        {
            return Effects[id];
        }
    }
}