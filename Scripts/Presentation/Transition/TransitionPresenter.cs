using System.Threading;
using Cysharp.Threading.Tasks;

namespace Unity1week202403.Presentation
{
    public class TransitionPresenter
    {
        private readonly TransitionView _view;

        public TransitionPresenter(TransitionView view)
        {
            _view = view;
        }

        public async UniTask ShowAsync(CancellationToken cancellationToken)
        {
            await _view.ShowAsync(cancellationToken);
        }

        public async UniTask HideAsync(CancellationToken cancellationToken)
        {
            await _view.HideAsync(cancellationToken);
        }
    }
}