using R3;
using Unity1week202403.Domain;
using Unity1week202403.Extensions;
using VContainer.Unity;

namespace Unity1week202403.Presentation
{
    public class CurrentCostPresenter : Presenter, IStartable
    {
        private readonly CurrentCostView _view;
        private readonly PlayerStatus _playerStatus;
        
        public CurrentCostPresenter(
            CurrentCostView view,
            PlayerStatus playerStatus)
        {
            _view = view;
            _playerStatus = playerStatus;
        }

        public void Start()
        {
            _playerStatus.CostStatus.ReactiveCurrent
                .Subscribe(cost => _view.UpdateCost(cost.Value))
                .AddTo(this);
        }
    }
}