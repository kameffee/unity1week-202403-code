using Unity1week202403.Structure;
using UnityEngine;

namespace Unity1week202403.Domain
{
    public class HistoryRecordData
    {
        public BattleMonsterId BattleMonsterId { get; }
        public MonsterId MonsterId { get; }
        public Vector3 Position { get; }

        public HistoryRecordData(BattleMonsterId battleMonsterId, MonsterId monsterId, Vector3 position)
        {
            BattleMonsterId = battleMonsterId;
            MonsterId = monsterId;
            Position = position;
        }
    }
}