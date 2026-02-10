using System.Collections.Generic;
using Runtime.Descriptions.StatusEffects.Enums;
using Runtime.Extensions;

namespace Runtime.Descriptions.StatusEffects
{
    public class DurationDescription
    {
        private const string TypeKey = "type";
        private const string TurnsKey = "turns";

        public DurationType Type { get; }
        public int Turns { get; }

        public DurationDescription(Dictionary<string, object> data)
        {
            Type = data.GetEnum<DurationType>(TypeKey);
            Turns = data.ContainsKey(TurnsKey) ? data.GetInt(TurnsKey) : 1;
        }
    }
}