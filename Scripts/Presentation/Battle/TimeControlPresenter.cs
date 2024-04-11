using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Unity1week202403.Domain;
using Unity1week202403.Extensions;
using UnityEngine;
using VContainer.Unity;

namespace Unity1week202403.Presentation
{
    public class TimeControlPresenter : Presenter, IInitializable
    {
        private readonly TimeControlUseCase _timeControlUseCase;
        private readonly InBattleTimeControlView _inBattleTimeControlView;

        public TimeControlPresenter(
            TimeControlUseCase timeControlUseCase,
            InBattleTimeControlView inBattleTimeControlView)
        {
            _timeControlUseCase = timeControlUseCase;
            _inBattleTimeControlView = inBattleTimeControlView;
        }

        public void Initialize()
        {
            _inBattleTimeControlView.OnPlayButtonClickAsObservable()
                .Subscribe(_ => _timeControlUseCase.Play())
                .AddTo(this);

            _inBattleTimeControlView.OnStopButtonClickAsObservable()
                .Subscribe(_ => _timeControlUseCase.Stop())
                .AddTo(this);

            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.Space))
                .Subscribe(_ => _timeControlUseCase.SwitchPlayAndStop())
                .AddTo(this);

            _timeControlUseCase.IsPlaying
                .Subscribe(isPlaying => _inBattleTimeControlView.SetState(isPlaying))
                .AddTo(this);

            _timeControlUseCase.TimeScale
                .Subscribe(timeScale => _inBattleTimeControlView.SetTimeScaleIndex(timeScale))
                .AddTo(this);

            _inBattleTimeControlView.OnSpeedChangedAsObservable()
                .Subscribe(index => _timeControlUseCase.SetTimeScale((TimeScaleType)index))
                .AddTo(this);
        }

        public async UniTask ShowAsync(CancellationToken cancellationToken)
        {
            await _inBattleTimeControlView.ShowAsync(cancellationToken);
        }

        public async UniTask HideAsync(CancellationToken cancellationToken)
        {
            await _inBattleTimeControlView.HideAsync(cancellationToken);
        }
    }
}