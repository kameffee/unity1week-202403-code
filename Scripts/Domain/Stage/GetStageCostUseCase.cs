using Unity1week202403.Data;
using Unity1week202403.Structure;

namespace Unity1week202403.Domain
{
    public class GetStageCostUseCase
    {
        private readonly StageMasterDataRepository _stageMasterDataRepository;

        public GetStageCostUseCase(
            StageMasterDataRepository stageMasterDataRepository)
        {
            _stageMasterDataRepository = stageMasterDataRepository;
        }

        public Cost Get(StageId stageId)
        {
            var stageMasterData = _stageMasterDataRepository.Get(stageId);

            return new Cost(stageMasterData.PlayerCost);
        }
    }
}