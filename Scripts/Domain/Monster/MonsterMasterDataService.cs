using Unity1week202403.Data;
using Unity1week202403.Structure;
using UnityEngine;

namespace Unity1week202403.Domain
{
    public class MonsterMasterDataService
    {
        private readonly MonsterMasterDataRepository _monsterMasterDataRepository;

        public MonsterMasterDataService(MonsterMasterDataRepository monsterMasterDataRepository)
        {
            _monsterMasterDataRepository = monsterMasterDataRepository;
        }

        public float GetColliderRadius(MonsterId monsterId)
        {
            var monsterMasterData = _monsterMasterDataRepository.Get(monsterId);
            var capsuleCollider = monsterMasterData.Prefab.GetComponentInChildren<CapsuleCollider>();
            return capsuleCollider.radius;
        }
    }
}