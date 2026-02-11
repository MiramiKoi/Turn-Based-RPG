using System.Collections.Generic;
using Runtime.Descriptions.StatusEffects.Enums;
using Runtime.Extensions;

namespace Runtime.Descriptions.StatusEffects
{
    public class StackingDescription
    {
        private const string ModeKey = "mode";
        private const string MaxStacksKey = "max_stacks";

        public StackingMode Mode { get; }
        public int MaxStacks { get; }

        public StackingDescription(Dictionary<string, object> data)
        {
            Mode = data.GetEnum<StackingMode>(ModeKey);

            MaxStacks = data.ContainsKey(MaxStacksKey) ? data.GetInt(MaxStacksKey) : 1;
        }
    }
}