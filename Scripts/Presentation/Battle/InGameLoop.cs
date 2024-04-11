using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity1week202403.Domain;
using Unity1week202403.Extensions;
using UnityEngine;
using VContainer.Unity;

namespace Unity1week202403.Presentation
{
    public class InGameLoop : Presenter, IAsyncStartable
    {
        private readonly PlayerStatus _playerStatus;
        private readonly EnemyGeneratePresenter _enemyGeneratePresenter;
        private readonly PlayerGeneratePresenter _playerGeneratePresenter;
        private readonly StageSituation _stageSituation;
        private readonly GetStageCostUseCase _getStageCostUseCase;
        private readonly BattleStartPerformPresenter _battleStartPerformPresenter;
        private readonly BattlePerformPresenter _battlePerformPresenter;
        private readonly BattleResultCalculator _battleResultCalculator;
        private readonly BattleResultPresenter _battleResultPresenter;
        private readonly BattleInitializer _battleInitializer;
        private readonly NextStageUseCase _nextStageUseCase;
        private readonly AudioPlayer _audioPlayer;
        private readonly LoadStageSceneUseCase _loadStageSceneUseCase;
        private readonly PlaceRecordUseCase _placeRecordUseCase;
        private readonly SceneLoader _sceneLoader;
        private readonly BattleShutdownUseCase _battleShutdownUseCase;
        private readonly BattleReadyPerformPresenter _battleReadyPerformPresenter;

        public InGameLoop(
            PlayerStatus playerStatus,
            EnemyGeneratePresenter enemyGeneratePresenter,
            PlayerGeneratePresenter playerGeneratePresenter,
            StageSituation stageSituation,
            GetStageCostUseCase getStageCostUseCase,
            BattleStartPerformPresenter battleStartPerformPresenter,
            BattlePerformPresenter battlePerformPresenter,
            BattleResultCalculator battleResultCalculator,
            BattleResultPresenter battleResultPresenter,
            BattleInitializer battleInitializer,
            NextStageUseCase nextStageUseCase,
            AudioPlayer audioPlayer,
            LoadStageSceneUseCase loadStageSceneUseCase,
            PlaceRecordUseCase placeRecordUseCase,
            SceneLoader sceneLoader,
            BattleShutdownUseCase battleShutdownUseCase,
            BattleReadyPerformPresenter battleReadyPerformPresenter)
        {
            _playerStatus = playerStatus;
            _enemyGeneratePresenter = enemyGeneratePresenter;
            _playerGeneratePresenter = playerGeneratePresenter;
            _stageSituation = stageSituation;
            _getStageCostUseCase = getStageCostUseCase;
            _battleStartPerformPresenter = battleStartPerformPresenter;
            _battlePerformPresenter = battlePerformPresenter;
            _battleResultCalculator = battleResultCalculator;
            _battleResultPresenter = battleResultPresenter;
            _battleInitializer = battleInitializer;
            _nextStageUseCase = nextStageUseCase;
            _audioPlayer = audioPlayer;
            _loadStageSceneUseCase = loadStageSceneUseCase;
            _placeRecordUseCase = placeRecordUseCase;
            _sceneLoader = sceneLoader;
            _battleShutdownUseCase = battleShutdownUseCase;
            _battleReadyPerformPresenter = battleReadyPerformPresenter;
        }

        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            var isReset = BattleReset.None;
            var isRetry = false;

            // 事前に表示しておく
            _battleReadyPerformPresenter.QuickShow();

            // シーン遷移待ち
            await _sceneLoader.WaitAsync(cancellationToken);

            while (!cancellationToken.IsCancellationRequested)
            {
                _audioPlayer.PlayBgm("InGame/Standby");

                _battleInitializer.Initialize();

                var stageId = _stageSituation.Get();
                Debug.Log("Start Stage: " + stageId.Value);

                // おためし
                {
                    // コストをステージごとに用意
                    _playerStatus.CostStatus.Consume(_playerStatus.CostStatus.Current);
                    var cost = _getStageCostUseCase.Get(stageId);
                    _playerStatus.CostStatus.Add(cost);
                }

                // ステージ生成
                await _loadStageSceneUseCase.LoadAsync(stageId, cancellationToken);

                // 敵生成
                _enemyGeneratePresenter.Generate(stageId);

                // ステージ名演出
                if (!isReset.Value && !isRetry)
                {
                    await _battleReadyPerformPresenter.PlayStageNamePerformAsync(stageId, cancellationToken);

                    // 扉を開ける
                    await _battleReadyPerformPresenter.HideAsync(cancellationToken);
                }

                await _playerGeneratePresenter.GenerateAsync(stageId, cancellationToken);

                await _battleStartPerformPresenter.ShowAsync(cancellationToken);

                // 戦闘
                await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: cancellationToken);
                _battleStartPerformPresenter.HideAsync(cancellationToken).Forget();

                // バトル開始SE
                _audioPlayer.PlaySe("InGame/BattleStart");

                isRetry = false;
                isReset = await _battlePerformPresenter.PerformAsync(stageId, cancellationToken);

                if (isReset.Value)
                {
                    continue;
                }

                var battleResult = _battleResultCalculator.Calculate();
                Debug.Log("Battle Result: " + battleResult);

                var nextAction = await _battleResultPresenter.ShowResultAsync(stageId, battleResult, cancellationToken);

                if (nextAction == BattleResultNextAction.Title)
                {
                    break;
                }

                if (nextAction == BattleResultNextAction.Retry)
                {
                    isRetry = true;
                    
                    // 何もしないとループ
                }
                else if (nextAction == BattleResultNextAction.NextStage)
                {
                    await _battleReadyPerformPresenter.ShowAsync(cancellationToken);

                    if (_nextStageUseCase.HasNext())
                    {
                        _nextStageUseCase.SetNext();
                        // 直前の編成データはクリア
                        _placeRecordUseCase.Reset();

                        await UniTask.Delay(TimeSpan.FromSeconds(1f),
                            ignoreTimeScale: true,
                            cancellationToken: cancellationToken);
                    }
                    else
                    {
                        // エンディングへ遷移させる
                        break;
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }

            Debug.Log("Game End");

            // バトル終了時の後処理
            _battleShutdownUseCase.Shutdown();

            GoToEndingAsync();
        }

        private void GoToEndingAsync()
        {
            _sceneLoader.LoadAsync(Const.Scene.Ending).Forget();
        }
    }
}