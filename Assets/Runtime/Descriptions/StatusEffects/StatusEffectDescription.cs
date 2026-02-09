using System.Collections.Generic;
using Runtime.Descriptions.StatusEffects.Enums;
using Runtime.Extensions;

namespace Runtime.Descriptions.StatusEffects
{
    public class StatusEffectDescription : Description
    {
        private const string CategoryKey = "category";
        private const string PolarityKey = "polarity";
        private const string StackingKey = "stacking";
        private const string DurationKey = "duration";
        private const string FlagsKey = "flags";

        public string ViewId { get; }
        public StatusCategory Category { get; }
        public Polarity Polarity { get; }
        public string LuaScript { get; set; }

        public StackingDescription Stacking { get; }
        public DurationDescription Duration { get; }

        public StatusFlag Flags { get; }

        public StatusEffectDescription(string id, Dictionary<string, object> data) : base(id)
        {
            ViewId = data.GetString("view_id");

            Category = data.GetEnum<StatusCategory>(CategoryKey);
            Polarity = data.GetEnum<Polarity>(PolarityKey);

            LuaScript = data.GetString("lua_script");

            Stacking = new StackingDescription(data.GetNode(StackingKey));
            Duration = new DurationDescription(data.GetNode(DurationKey));

            Flags = data.GetFlags<StatusFlag>(FlagsKey);
        }
    }
}