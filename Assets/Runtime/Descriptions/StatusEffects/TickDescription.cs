using System.Collections.Generic;
using Runtime.Descriptions.StatusEffects.Enums;
using Runtime.Extensions;

namespace Runtime.Descriptions.StatusEffects
{
    public class TickDescription
    {
        private const string OnKey = "on";

        public TickMoment On { get; }

        public TickDescription(Dictionary<string, object> data)
        {
            On = data.GetEnum<TickMoment>(OnKey);
        }
    }
}