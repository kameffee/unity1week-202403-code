using System.Threading;
using Cysharp.Threading.Tasks;
using Unity1week202403.Data;
using Unity1week202403.Extensions;
using Unity1week202403.Structure;

namespace Unity1week202403.Presentation
{
    public class BattleReadyPerformPresenter : Presenter
    {
        private readonly BattleReadyPerformView _view;
        private readonly StageMasterDataRepository _stageMasterDataRepository;

        public BattleReadyPerformPresenter(
            BattleReadyPerformView view,
            StageMasterDataRepository stageMasterDataRepository)
        {
            _view = view;
            _stageMasterDataRepository = stageMasterDataRepository;
        }

        public void QuickShow()
        {
            _view.QuickShow();
        }

        public async UniTask ShowAsync(CancellationToken cancellationToken = default)
        {
            await _view.ShowAsync(cancellationToken);
        }

        public async UniTask PlayStageNamePerformAsync(StageId stageId, CancellationToken cancellationToken = default)
        {
            var stageMasterData = _stageMasterDataRepository.Get(stageId);
            await _view.PlayStageNamePerform(stageMasterData.StageName, cancellationToken);
        }

        public async UniTask HideAsync(CancellationToken cancellationToken = default)
        {
            await _view.HideAsync(cancellationToken);
        }
    }
}