using System.Collections.Generic;

namespace Unity1week202403.Data
{
    public readonly struct StageInfo
    {
        private readonly List<MonsterGenerateSet> _monsterGenerateSetList;
        public IEnumerable<MonsterGenerateSet> MonsterGenerateSets => _monsterGenerateSetList;

        public StageInfo(IEnumerable<MonsterGenerateSet> monsterPositionInfoList)
        {
            _monsterGenerateSetList = new List<MonsterGenerateSet>();
            _monsterGenerateSetList.AddRange(monsterPositionInfoList);
        }
    }
}