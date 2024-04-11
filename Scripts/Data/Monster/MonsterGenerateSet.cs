using Unity1week202403.Structure;
using UnityEngine;

namespace Unity1week202403.Data
{
    public readonly struct MonsterGenerateSet
    {
        public MonsterId MonsterId { get; }
        public Vector3 Position { get; }
        public GameObject Prefab { get; }

        public MonsterGenerateSet(MonsterId monsterId, Vector3 position, GameObject prefab)
        {
            MonsterId = monsterId;
            Position = position;
            Prefab = prefab;
        }
    }
}