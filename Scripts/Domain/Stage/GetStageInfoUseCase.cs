using System.Linq;
using Unity1week202403.Data;
using Unity1week202403.Structure;
using UnityEngine;

namespace Unity1week202403.Domain
{
    public class GetStageInfoUseCase
    {
        private readonly StageMasterDataRepository _stageMasterDataRepository;

        public GetStageInfoUseCase(
            StageMasterDataRepository stageMasterDataRepository)
        {
            _stageMasterDataRepository = stageMasterDataRepository;
        }

        public StageInfo Get(StageId stageId)
        {
            var stageMasterData = _stageMasterDataRepository.Get(stageId);

            var monsterGenerateSets = stageMasterData.StageMonsterData
                .Select(monsterData => CreateMonsterGenerateSet(monsterData, monsterData.Position));

            return new StageInfo(monsterGenerateSets);
        }

        private MonsterGenerateSet CreateMonsterGenerateSet(StageMonsterData monsterData, Vector3 position)
        {
            var monsterMasterData = monsterData.MonsterMasterData;
            return new MonsterGenerateSet(
                monsterMasterData.Id,
                position,
                monsterMasterData.Prefab);
        }
    }
}