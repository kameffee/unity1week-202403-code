using Unity1week202403.Data;
using Unity1week202403.Structure;

namespace Unity1week202403.Presentation
{
    using ViewModel = MonsterDetailView.ViewModel;

    public class CreateMonsterDetailViewModelUseCase
    {
        private readonly MonsterMasterDataRepository _monsterMasterDataRepository;

        public CreateMonsterDetailViewModelUseCase(MonsterMasterDataRepository monsterMasterDataRepository)
        {
            _monsterMasterDataRepository = monsterMasterDataRepository;
        }

        public ViewModel Create(MonsterId monsterId)
        {
            var masterData = _monsterMasterDataRepository.Get(monsterId);
            return new ViewModel(
                masterData.Thumbnail,
                masterData.Name,
                masterData.BattleFeature,
                masterData.Description);
        }
    }
}