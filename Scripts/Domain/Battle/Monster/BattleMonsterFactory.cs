using Unity1week202403.Data;
using Unity1week202403.Structure;

namespace Unity1week202403.Domain
{
    public class BattleMonsterFactory
    {
        private readonly MonsterMasterDataRepository _monsterMasterDataRepository;

        public BattleMonsterFactory(MonsterMasterDataRepository monsterMasterDataRepository)
        {
            _monsterMasterDataRepository = monsterMasterDataRepository;
        }

        public BattleMonster Create(BattleMonsterId battleMonsterId, MonsterId monsterId, bool isAlly)
        {
            var monsterMasterData = _monsterMasterDataRepository.Get(monsterId);

            var activeSkill = monsterMasterData.SkillMasterData != null
                ? new ActiveSkill(monsterMasterData.SkillMasterData)
                : ActiveSkill.DefaultSkill;

            return new BattleMonster(
                battleMonsterId,
                monsterId,
                isAlly,
                monsterMasterData.Cost,
                monsterMasterData.Parameter,
                activeSkill,
                monsterMasterData.ZPosition);
        }
    }
}