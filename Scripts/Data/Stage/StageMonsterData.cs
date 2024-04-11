using System;
using UnityEngine;

namespace Unity1week202403.Data
{
    [Serializable]
    public class StageMonsterData
    {
        public MonsterMasterData MonsterMasterData => _monsterMasterData;
        public Vector3 Position => _position;

        [SerializeField]
        private MonsterMasterData _monsterMasterData;

        [SerializeField]
        private Vector3 _position;
    }
}