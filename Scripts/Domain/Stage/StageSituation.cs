using Unity1week202403.Structure;

namespace Unity1week202403.Domain
{
    public class StageSituation
    {
        private StageId _stageId;

        public StageSituation(StageId initialStageId)
        {
            _stageId = initialStageId;
        }

        public void Set(StageId stageId)
        {
            _stageId = stageId;
        }

        public StageId Get() => _stageId;
    }
}