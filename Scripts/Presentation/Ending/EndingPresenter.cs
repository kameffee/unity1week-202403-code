using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Unity1week202403.Domain;
using Unity1week202403.Domain.Capture;
using Unity1week202403.Extensions;
using VContainer.Unity;

namespace Unity1week202403.Presentation
{
    public class EndingPresenter : Presenter, IStartable, IAsyncStartable
    {
        private readonly EndingView _view;
        private readonly SceneLoader _sceneLoader;
        private readonly CaptureTextureContainer _captureTextureContainer;
        private readonly EndingCardView _endingCardView;
        private readonly CreateEndingCardViewModel _createEndingCardViewModel;

        public EndingPresenter(
            EndingView view,
            SceneLoader sceneLoader,
            CaptureTextureContainer captureTextureContainer,
            EndingCardView endingCardView,
            CreateEndingCardViewModel createEndingCardViewModel)
        {
            _view = view;
            _sceneLoader = sceneLoader;
            _captureTextureContainer = captureTextureContainer;
            _endingCardView = endingCardView;
            _createEndingCardViewModel = createEndingCardViewModel;
        }

        public void Start()
        {
            _view.OnClickBackToTitleAsObservable()
                .Subscribe(_ => BackToTitle())
                .AddTo(this);

            var viewModel = _createEndingCardViewModel.Create();
            _endingCardView.ApplyViewModel(viewModel);
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(2), cancellationToken: cancellation);

            await _endingCardView.ShowAsync(cancellation);

            await _endingCardView.PerformAsync(cancellation);

            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellation);

            await _endingCardView.HideAsync(cancellation);

            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellation);

            await _view.ShowAsync(cancellation);
        }

        private void BackToTitle()
        {
            // スクショのキャッシュを全削除
            _captureTextureContainer.Clear();

            _sceneLoader.LoadAsync(Const.Scene.Title).Forget();
        }
    }
}