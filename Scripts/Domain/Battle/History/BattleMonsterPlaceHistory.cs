namespace Unity1week202403.Domain
{
    public class BattleMonsterPlaceHistory
    {
        private BattleMonsterPlaceRecord _lastRecord;

        public bool HasRecord() => _lastRecord != null;

        public void Set(BattleMonsterPlaceRecord historyRecordData)
        {
            _lastRecord = historyRecordData;
        }

        public BattleMonsterPlaceRecord GetLast() => _lastRecord;

        public void Reset() => _lastRecord = null;
    }
}