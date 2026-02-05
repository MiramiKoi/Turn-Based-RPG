using System;

namespace Runtime.Descriptions.StatusEffects.Enums
{
    [Flags]
    public enum StatusFlag
    {
        None = 0,
        Physical = 1 << 0,
        Dot = 1 << 1,
        Fire = 1 << 2,
        Mental = 1 << 3,
        Persistent = 1 << 4
    }
}