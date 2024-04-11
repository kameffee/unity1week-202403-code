using System.Collections.Generic;
using Unity1week202403.Presentation;

namespace Unity1week202403.Domain
{
    public class BattleMonsterPresenterContainer
    {
        private readonly List<BattleMonsterPresenter> _monsters = new();

        public void Add(BattleMonsterPresenter monster) => _monsters.Add(monster);
        public IReadOnlyList<BattleMonsterPresenter> GetAll() => _monsters;

        public void ClearAndDestroy()
        {
            _monsters.ForEach(monster => UnityEngine.Object.Destroy(monster.gameObject));
            _monsters.Clear();
        }

        public void RemoveAndDestroy(params BattleMonsterId[] battleMonsterId)
        {
            foreach (var id in battleMonsterId)
            {
                var monster = _monsters.Find(presenter => presenter.BattleMonsterId == id);
                _monsters.Remove(monster);
                UnityEngine.Object.Destroy(monster.gameObject);
            }
        }

        public BattleMonsterPresenter Get(BattleMonsterId id)
        {
            return _monsters.Find(presenter => presenter.BattleMonsterId == id);
        }
    }
}