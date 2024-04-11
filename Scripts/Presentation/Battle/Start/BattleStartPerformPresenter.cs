using System.Threading;
using Cysharp.Threading.Tasks;
using Unity1week202403.Extensions;

namespace Unity1week202403.Presentation
{
    public class BattleStartPerformPresenter : Presenter
    {
        private readonly BattleStartPerformView _view;

        public BattleStartPerformPresenter(BattleStartPerformView view)
        {
            _view = view;
        }

        public async UniTask ShowAsync(CancellationToken cancellationToken = default)
        {
            await _view.ShowAsync(cancellationToken);
        }

        public async UniTask HideAsync(CancellationToken cancellationToken = default)
        {
            await _view.HideAsync(cancellationToken);
        }
    }
}