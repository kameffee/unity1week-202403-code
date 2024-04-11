using Unity1week202403.Presentation;
using UnityEngine;

namespace Unity1week202403.Domain
{
    public class FindPlacedMonsterUseCase
    {
        private readonly BattleMonsterContainer _battleMonsterContainer;

        public FindPlacedMonsterUseCase(BattleMonsterContainer battleMonsterContainer)
        {
            _battleMonsterContainer = battleMonsterContainer;
        }

        public bool TryAllyFind(Vector3 worldPosition, out BattleMonster battleMonster)
        {
            if (Physics.Raycast(
                    origin: worldPosition + Vector3.up * 100,
                    direction: Vector3.down,
                    out var hit,
                    maxDistance: 100,
                    layerMask: Const.LayerMaskMonsterCollider))
            {
                if (hit.transform.TryGetComponent<BattleMonsterPresenter>(out var battleMonsterPresenter))
                {
                    var monster = _battleMonsterContainer.Get(battleMonsterPresenter.BattleMonsterId);

                    if (monster.IsAlly)
                    {
                        battleMonster = monster;
                        return true;
                    }
                }
            }

            battleMonster = default;
            return false;
        }
    }
}