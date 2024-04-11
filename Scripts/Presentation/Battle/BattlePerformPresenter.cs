using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Unity1week202403.Domain;
using Unity1week202403.Domain.Capture;
using Unity1week202403.Extensions;
using Unity1week202403.Structure;
using UnityEngine;
using VContainer.Unity;

namespace Unity1week202403.Presentation
{
    public class BattlePerformPresenter : Presenter, IInitializable
    {
        private readonly BattleMonsterPresenterContainer _battleMonsterPresenterContainer;
        private readonly BattleTerminationCalculator _battleTerminationCalculator;
        private readonly TimeControlPresenter _timeControlPresenter;
        private readonly InBattleUIView _inBattleUIView;

        private bool _isRetry;

        public BattlePerformPresenter(
            BattleMonsterPresenterContainer battleMonsterPresenterContainer,
            BattleTerminationCalculator battleTerminationCalculator,
            TimeControlPresenter timeControlPresenter,
            InBattleUIView inBattleUIView)
        {
            _battleMonsterPresenterContainer = battleMonsterPresenterContainer;
            _battleTerminationCalculator = battleTerminationCalculator;
            _timeControlPresenter = timeControlPresenter;
            _inBattleUIView = inBattleUIView;
        }

        public void Initialize()
        {
            var rKeyDownObservable = Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.R));

            _inBattleUIView.OnClickRetryAsObservable()
                .Merge(rKeyDownObservable)
                .Subscribe(_ => _isRetry = true)
                .AddTo(this);
        }

        public async UniTask<BattleReset> PerformAsync(StageId stageId, CancellationToken cancellationToken)
        {
            _isRetry = false;

            _inBattleUIView.ShowAsync(cancellationToken).Forget();
            _timeControlPresenter.ShowAsync(cancellationToken).Forget();

            while (!_battleTerminationCalculator.IsBattleTerminated() && !cancellationToken.IsCancellationRequested)
            {
                if (_isRetry)
                {
                    OnEndBattle();
                    return new BattleReset(true);
                }

                // 全モンスターの行動/更新
                var monsters = _battleMonsterPresenterContainer.GetAll();
                foreach (var monster in monsters)
                {
                    monster.Perform();
                }

                await UniTask.Yield(cancellationToken: cancellationToken);
            }

            OnEndBattle();

            return new BattleReset(false);
        }

        private void OnEndBattle()
        {
            _timeControlPresenter.HideAsync(CancellationToken.None).Forget();
            _inBattleUIView.HideAsync(CancellationToken.None).Forget();
        }
    }
}