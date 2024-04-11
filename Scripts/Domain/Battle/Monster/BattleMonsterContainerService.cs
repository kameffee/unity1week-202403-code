
using R3;
using Unity1week202403.Presentation;
using UnityEngine;
using VContainer.Unity;

namespace Unity1week202403.Domain
{
    public class BattleMonsterContainerService : IInitializable
    {
        private readonly BattleMonsterContainer _battleMonsterContainer;
        private readonly BattleMonsterPresenterContainer _battleMonsterPresenterContainer;
        private BattleMonster _lastDeadMonster;

        public BattleMonsterContainerService(
            BattleMonsterContainer battleMonsterContainer,
            BattleMonsterPresenterContainer battleMonsterPresenterContainer)
        {
            _battleMonsterContainer = battleMonsterContainer;
            _battleMonsterPresenterContainer = battleMonsterPresenterContainer;
        }

        public void Initialize()
        {
            _battleMonsterContainer.OnDead.Subscribe(monster =>
            {
                Debug.Log("Dead: " + monster.BattleMonsterId.Value);
                _lastDeadMonster = monster;
            });
        }

        public BattleMonsterPresenter GetLastDeadMonster()
        {
            return _battleMonsterPresenterContainer.Get(_lastDeadMonster.BattleMonsterId);
        }
    }
}