using Unity1week202403.Data;
using Unity1week202403.Structure;

namespace Unity1week202403.Domain
{
    public class PlayerConsumeCostUseCase
    {
        private readonly PlayerStatus _playerStatus;
        private readonly MonsterMasterDataRepository _monsterMasterDataRepository;

        public PlayerConsumeCostUseCase(
            PlayerStatus playerStatus,
            MonsterMasterDataRepository monsterMasterDataRepository)
        {
            _playerStatus = playerStatus;
            _monsterMasterDataRepository = monsterMasterDataRepository;
        }

        public void Consume(MonsterId monsterId)
        {
            var monster = _monsterMasterDataRepository.Get(monsterId);
            _playerStatus.CostStatus.Consume(monster.Cost);
        }

        public bool CanConsume(MonsterId monsterId)
        {
            var monster = _monsterMasterDataRepository.Get(monsterId);
            return _playerStatus.CostStatus.Current.Value >= monster.Cost.Value;
        }
    }
}