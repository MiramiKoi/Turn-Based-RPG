using System.Collections.Generic;
using Runtime.Descriptions.StatusEffects.Enums;
using Runtime.Extensions;

namespace Runtime.Descriptions.StatusEffects
{
    public class StatusEffectDescription : Description
    {
        private const string PolarityKey = "polarity";
        private const string StackingKey = "stacking";
        private const string DurationKey = "duration";
        public string ViewId { get; }
        public Polarity Polarity { get; }
        public string LuaScript { get; }

        public StackingDescription Stacking { get; }
        public DurationDescription Duration { get; }

        public StatusEffectDescription(string id, Dictionary<string, object> data) : base(id)
        {
            ViewId = data.GetString("view_id");

            Polarity = data.GetEnum<Polarity>(PolarityKey);

            LuaScript = data.GetString("lua_script");

            Stacking = new StackingDescription(data.GetNode(StackingKey));
            Duration = new DurationDescription(data.GetNode(DurationKey));
        }
    }
}