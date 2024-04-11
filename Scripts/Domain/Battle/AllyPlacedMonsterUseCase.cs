using System.Linq;
using Unity1week202403.Data;

namespace Unity1week202403.Domain
{
    public class AllyPlacedMonsterUseCase
    {
        private readonly BattleMonsterContainer _battleMonsterContainer;
        private readonly BattleMonsterPresenterContainer _battleMonsterPresenterContainer;
        private readonly MonsterMasterDataRepository _monsterMasterDataRepository;
        private readonly PlayerStatus _playerStatus;

        public AllyPlacedMonsterUseCase(
            BattleMonsterContainer battleMonsterContainer,
            BattleMonsterPresenterContainer battleMonsterPresenterContainer,
            MonsterMasterDataRepository monsterMasterDataRepository,
            PlayerStatus playerStatus)
        {
            _battleMonsterContainer = battleMonsterContainer;
            _battleMonsterPresenterContainer = battleMonsterPresenterContainer;
            _monsterMasterDataRepository = monsterMasterDataRepository;
            _playerStatus = playerStatus;
        }

        public void ResetAll()
        {
            foreach (var battleMonsterId in _battleMonsterContainer.GetAllyIds().ToArray())
            {
                Remove(battleMonsterId);
            }
        }

        public void Remove(BattleMonsterId battleMonsterId)
        {
            var battleMonster = _battleMonsterContainer.Get(battleMonsterId);
            var masterData = _monsterMasterDataRepository.Get(battleMonster.MonsterId);

            _battleMonsterPresenterContainer.RemoveAndDestroy(battleMonsterId);
            _battleMonsterContainer.Remove(battleMonsterId);

            // コストを返却
            _playerStatus.CostStatus.Add(masterData.Cost);
        }
    }
}