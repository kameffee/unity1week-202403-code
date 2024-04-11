using System.Collections;
using System.Collections.Generic;

namespace Unity1week202403.Domain
{
    public class BattleMonsterPlaceRecord
    {
        private readonly List<HistoryRecordData> _records = new();

        public void AddRecord(HistoryRecordData historyRecordData)
        {
            _records.Add(historyRecordData);
        }

        public IEnumerable<HistoryRecordData> GetRecords() => _records;
    }
}