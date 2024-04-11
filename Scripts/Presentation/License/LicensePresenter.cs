using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Unity1week202403.Extensions;

namespace Unity1week202403.Presentation
{
    public class LicensePresenter : Presenter
    {
        private readonly Func<LicenseView> _viewFactory;
        private readonly GetLicenseTextUseCase _getLicenseTextUseCase;

        private LicenseView _view;

        public LicensePresenter(
            Func<LicenseView> viewFactory,
            GetLicenseTextUseCase getLicenseTextUseCase)
        {
            _viewFactory = viewFactory;
            _getLicenseTextUseCase = getLicenseTextUseCase;
        }

        private void OnSubscribe(LicenseView view)
        {
            view.SetLicenseText(_getLicenseTextUseCase.Get());
            view.OnCloseAsObservable()
                .Subscribe(_ => view.HideAsync().Forget())
                .AddTo(this)
                .AddTo(view);
        }

        public async UniTask ShowAsync(CancellationToken cancellationToken = default)
        {
            if (_view == null)
            {
                _view = _viewFactory.Invoke();
                OnSubscribe(_view);
            }

            await _view.ShowAsync(cancellationToken);
        }

        public async UniTask HideAsync(CancellationToken cancellationToken = default)
        {
            await _view.HideAsync(cancellationToken);
        }
    }
}