using Unity1week202403.Data;
using Unity1week202403.Structure;

namespace Unity1week202403.Domain
{
    public class NextStageUseCase
    {
        private readonly StageSituation _stageSituation;
        private readonly StageMasterDataRepository _stageMasterDataRepository;
        
        public NextStageUseCase(
            StageSituation stageSituation,
            StageMasterDataRepository stageMasterDataRepository)
        {
            _stageSituation = stageSituation;
            _stageMasterDataRepository = stageMasterDataRepository;
        }

        public bool HasNext()
        {
            var currentStageId = _stageSituation.Get();
            var nextStageId = new StageId(currentStageId.Value + 1);
            return _stageMasterDataRepository.Exists(nextStageId);
        }

        public StageId SetNext()
        {
            var currentStageId = _stageSituation.Get();
            var nextStageId = new StageId(currentStageId.Value + 1);
            _stageMasterDataRepository.Exists(nextStageId);
            _stageSituation.Set(nextStageId);
            return nextStageId;
        }
    }
}