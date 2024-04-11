using System.Linq;
using Unity1week202403.Data;
using Unity1week202403.Presentation;
using Unity1week202403.Structure;

namespace Unity1week202403.Domain
{
    using ViewModel = MonsterSelectView.ViewModel;
    using ElementViewModel = MonsterSelectElementView.ViewModel;

    public class CreateMonsterSelectViewModelUseCase
    {
        private readonly StageMasterDataStoreSource _stageMasterDataStoreSource;
        private readonly MonsterMasterDataRepository _monsterMasterDataRepository;

        public CreateMonsterSelectViewModelUseCase(StageMasterDataStoreSource stageMasterDataStoreSource, MonsterMasterDataRepository monsterMasterDataRepository)
        {
            _stageMasterDataStoreSource = stageMasterDataStoreSource;
            _monsterMasterDataRepository = monsterMasterDataRepository;
        }

        public ViewModel Create(StageId stageId)
        {
            var elementViewModels = _stageMasterDataStoreSource.Get(stageId)
                .PlayerPlaceableMonsterList.Select(x => _monsterMasterDataRepository.Get(new MonsterId(x)))
                .Select(CreateElementViewModel)
                .ToArray();

            return new ViewModel(elementViewModels);
        }

        private ElementViewModel CreateElementViewModel(MonsterMasterData data)
        {
            return new ElementViewModel(
                data.Id,
                data.Thumbnail,
                data.Cost,
                data.BigMonsterMasterData != null
                    ? new ElementViewModel(
                        data.BigMonsterMasterData.Id,
                        data.BigMonsterMasterData.Thumbnail,
                        data.BigMonsterMasterData.Cost)
                    : null);
        }
    }
}