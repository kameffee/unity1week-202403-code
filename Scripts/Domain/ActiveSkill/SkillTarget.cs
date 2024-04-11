using System;
using System.Collections.Generic;
using System.Linq;

namespace Unity1week202403.Domain
{
    public class SkillTarget
    {
        public BattleMonsterId MainTarget { get; }
        public IReadOnlyCollection<BattleMonsterId> SubTargets { get; }
        public IEnumerable<BattleMonsterId> AllTargets => SubTargets.Prepend(MainTarget);

        public SkillTarget(BattleMonsterId mainTarget, IEnumerable<BattleMonsterId> subTargets = null)
        {
            MainTarget = mainTarget;
            SubTargets = subTargets?.ToArray() ?? Array.Empty<BattleMonsterId>();
        }
    }
}