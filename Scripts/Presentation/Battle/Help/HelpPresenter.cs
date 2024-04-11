using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Unity1week202403.Extensions;
using VContainer.Unity;

namespace Unity1week202403.Presentation
{
    public class HelpPresenter : Presenter, IInitializable
    {
        private readonly HelpView _helpView;
        private readonly MonsterSelectView _monsterSelectView;
        private readonly InBattleUIView _inBattleUIView;

        public HelpPresenter(
            HelpView helpView,
            MonsterSelectView monsterSelectView,
            InBattleUIView inBattleUIView)
        {
            _helpView = helpView;
            _monsterSelectView = monsterSelectView;
            _inBattleUIView = inBattleUIView;
        }

        public void Initialize()
        {
            _inBattleUIView.OnClickHelpAsObservable()
                .Merge(_monsterSelectView.OnHelpAsObservable())
                .SubscribeAwait((_, token) => ShowAsync(token))
                .AddTo(this);

            _helpView.OnClickCloseAsObservable()
                .SubscribeAwait((_, token) => HideAsync(token))
                .AddTo(this);
        }

        public async UniTask ShowAsync(CancellationToken cancellationToken = default)
        {
            await _helpView.ShowAsync(cancellationToken);
        }

        private async UniTask HideAsync(CancellationToken cancellationToken = default)
        {
            await _helpView.HideAsync(cancellationToken);
        }
    }
}