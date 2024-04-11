using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using Unity1week202403.Domain;
using Unity1week202403.Domain.Capture;
using Unity1week202403.Extensions;
using Unity1week202403.Structure;
using UnityEngine;

namespace Unity1week202403.Presentation
{
    public class BattleResultPresenter : Presenter
    {
        private readonly BattleResultFailedPerformView _viewResultFailedPerformView;
        private readonly BattleResultVictoryPerformView _resultVictoryPerformView;
        private readonly ResultVirtualCamera _resultVirtualCamera;
        private readonly BattleMonsterContainerService _battleMonsterContainerService;
        private readonly CaptureUseCase _captureUseCase;

        public BattleResultPresenter(
            BattleResultFailedPerformView viewResultFailedPerformView,
            BattleResultVictoryPerformView resultVictoryPerformView,
            ResultVirtualCamera resultVirtualCamera,
            BattleMonsterContainerService battleMonsterContainerService,
            CaptureUseCase captureUseCase)
        {
            _viewResultFailedPerformView = viewResultFailedPerformView;
            _resultVictoryPerformView = resultVictoryPerformView;
            _resultVirtualCamera = resultVirtualCamera;
            _battleMonsterContainerService = battleMonsterContainerService;
            _captureUseCase = captureUseCase;
        }

        public async UniTask<BattleResultNextAction> ShowResultAsync(
            StageId stageId,
            BattleResult battleResult,
            CancellationToken cancellationToken)
        {
            // 徐々にスローモーションにする
            await DOTween.To(
                    getter: () => Time.timeScale,
                    setter: value => Time.timeScale = value,
                    endValue: 0f,
                    // プレイ速度によって止まるタイミングがズレるためTimescaleで補正
                    duration: 1f / Time.timeScale)
                .SetEase(Ease.Linear)
                .SetUpdate(true)
                .WithCancellation(cancellationToken);

            // 最後に倒した敵の位置にカメラを移動
            var lastDeadMonsterPresenter = _battleMonsterContainerService.GetLastDeadMonster();
            _resultVirtualCamera.SetFollowTarget(lastDeadMonsterPresenter.transform);

            // カメラを切り替える
            _resultVirtualCamera.SetActive(true);

            // // カメラが移動しきるまで待機
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f), DelayType.UnscaledDeltaTime, cancellationToken: cancellationToken);

            // スクリーンショットを撮る
            await _captureUseCase.Capture(stageId);

            var nextAction = await PerformAsync(battleResult, cancellationToken);

            _resultVirtualCamera.SetActive(false);
            _resultVirtualCamera.SetFollowTarget(null);

            return nextAction;
        }

        private async UniTask<BattleResultNextAction> PerformAsync(BattleResult battleResult,
            CancellationToken cancellationToken)
        {
            switch (battleResult)
            {
                case BattleResult.Victory:
                    await _resultVictoryPerformView.ShowAsync(cancellationToken);
                    await _resultVictoryPerformView.OnClickNextAsObservable()
                        .FirstAsync(cancellationToken: cancellationToken);
                    await _resultVictoryPerformView.HideAsync(cancellationToken);
                    _resultVirtualCamera.SetActive(false);
                    return BattleResultNextAction.NextStage;
                case BattleResult.Defeat:
                    await _viewResultFailedPerformView.ShowAsync(cancellationToken);
                    await _viewResultFailedPerformView.OnClickRetryAsObservable()
                        .FirstAsync(cancellationToken: cancellationToken);
                    await _viewResultFailedPerformView.HideAsync(cancellationToken);
                    _resultVirtualCamera.SetActive(false);
                    return BattleResultNextAction.Retry;
                default:
                    throw new ArgumentOutOfRangeException(nameof(battleResult), battleResult, null);
            }
        }
    }
}