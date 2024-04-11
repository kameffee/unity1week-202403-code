using System;

namespace Unity1week202403.Data
{
    [Flags]
    public enum SkillTargetType
    {
        Self = 1 << 0,
        Ally = 1 << 1,
        Enemy = 1 << 2,
        SelfAndAlly = Self | Ally,
        All = Self | Ally | Enemy,
    }
}