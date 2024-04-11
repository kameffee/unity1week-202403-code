using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Unity1week202403.Extensions;
using Unity1week202403.Structure;
using VContainer.Unity;

namespace Unity1week202403.Presentation
{
    public class MonsterDetailPresenter : Presenter, IInitializable
    {
        private readonly MonsterDetailView _monsterDetailView;
        private readonly CreateMonsterDetailViewModelUseCase _createMonsterDetailViewModelUseCase;

        public MonsterDetailPresenter(
            MonsterDetailView monsterDetailView,
            CreateMonsterDetailViewModelUseCase createMonsterDetailViewModelUseCase)
        {
            _monsterDetailView = monsterDetailView;
            _createMonsterDetailViewModelUseCase = createMonsterDetailViewModelUseCase;
        }

        public void Initialize()
        {
            _monsterDetailView.OnClickCloseAsObservable()
                .SubscribeAwait((_, token) => HideAsync(token))
                .AddTo(this);
        }

        public async UniTask ShowAsync(MonsterId monsterId, CancellationToken cancellationToken = default)
        {
            var viewModel = _createMonsterDetailViewModelUseCase.Create(monsterId);
            _monsterDetailView.ApplyViewModel(viewModel);
            await _monsterDetailView.ShowAsync(cancellationToken);
        }

        private async UniTask HideAsync(CancellationToken cancellationToken = default)
        {
            await _monsterDetailView.HideAsync(cancellationToken);
        }
    }
}