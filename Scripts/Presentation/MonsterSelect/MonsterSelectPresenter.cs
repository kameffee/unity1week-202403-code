using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Unity1week202403.Domain;
using Unity1week202403.Extensions;
using Unity1week202403.Structure;
using VContainer.Unity;

namespace Unity1week202403.Presentation
{
    public class MonsterSelectPresenter : Presenter, IStartable
    {
        private readonly MonsterSelectView _view;
        private readonly CreateMonsterSelectViewModelUseCase _createMonsterSelectViewModelUseCase;
        private readonly PlayerStatus _playerStatus;
        private readonly BattleMonsterContainer _battleMonsterContainer;
        private readonly MonsterDetailPresenter _monsterDetailPresenter;

        public MonsterSelectPresenter(
            MonsterSelectView view,
            CreateMonsterSelectViewModelUseCase createMonsterSelectViewModelUseCase,
            PlayerStatus playerStatus,
            BattleMonsterContainer battleMonsterContainer,
            MonsterDetailPresenter monsterDetailPresenter)
        {
            _view = view;
            _createMonsterSelectViewModelUseCase = createMonsterSelectViewModelUseCase;
            _playerStatus = playerStatus;
            _battleMonsterContainer = battleMonsterContainer;
            _monsterDetailPresenter = monsterDetailPresenter;
        }

        public void Start()
        {
            _playerStatus.CostStatus.ReactiveCurrent
                .Subscribe(UpdateView)
                .AddTo(this);

            _view.SetActiveSubmitButton(_battleMonsterContainer.AnyAlly());

            _view.OnDetailAsObservable()
                .SubscribeAwait(async (monsterId, token) =>
                {
                    await _monsterDetailPresenter.ShowAsync(monsterId, token);
                })
                .AddTo(this);
        }

        public Observable<Unit> OnSubmitAsObservable() => _view.OnSubmitAsObservable();
        public Observable<Unit> OnResetAsObservable() => _view.OnResetAsObservable();
        public Observable<MonsterId> OnSelectAsObservable() => _view.OnSelectAsObservable();
        public Observable<Unit> OnBigSwitchAsObservable() => _view.OnBigSwitchAsObservable();

        private void UpdateView(Cost usableCost)
        {
            _view.UpdateUsableCost(usableCost);
            _view.SetActiveSubmitButton(_battleMonsterContainer.AnyAlly());
        }

        public async UniTask ShowAsync(StageId stageId, CancellationToken cancellationToken = default)
        {
            // ここで一覽を更新
            var viewModel = _createMonsterSelectViewModelUseCase.Create(stageId);
            _view.ApplyViewModel(_playerStatus.CostStatus.Current, viewModel);

            await _view.ShowAsync(cancellationToken);
        }

        public async UniTask HideAsync(CancellationToken cancellationToken = default)
        {
            await _view.HideAsync(cancellationToken);
        }

        public void SwitchBig() => _view.SwitchBig();
        public void Select(MonsterId monsterId) => _view.Select(monsterId);
        public void Deselect() => _view.Deselect();
    }
}