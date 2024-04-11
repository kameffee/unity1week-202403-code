using System.Linq;
using Unity1week202403.Structure;
using UnityEngine.Assertions;

namespace Unity1week202403.Domain
{
    public class CreateEnemyBattleMonsterUseCase
    {
        private readonly BattleMonsterContainer _battleMonsterContainer;
        private readonly BattleMonsterFactory _battleMonsterFactory;

        public CreateEnemyBattleMonsterUseCase(
            BattleMonsterContainer battleMonsterContainer,
            BattleMonsterFactory battleMonsterFactory)
        {
            _battleMonsterContainer = battleMonsterContainer;
            _battleMonsterFactory = battleMonsterFactory;
        }

        public BattleMonster CreateAndRegister(MonsterId monsterId)
        {
            var uniqueBattleMonsterId = CreateUniqueBattleMonsterId();
            var monster = _battleMonsterFactory.Create(uniqueBattleMonsterId, monsterId, false);
            _battleMonsterContainer.Add(monster);
            return monster;
        }

        private BattleMonsterId CreateUniqueBattleMonsterId()
        {
            var ids = _battleMonsterContainer.GetEnemyIds().ToArray();
            BattleMonsterId? id = null;

            //  重複しないIDを生成する
            for (int i = 0; i < 999; i++)
            {
                // 候補
                var candidate = BattleMonsterId.CreateEnemy(i);
                if (!ids.Contains(candidate))
                {
                    id = candidate;
                    break;
                }
            }

            Assert.IsTrue(id.HasValue, "IDが生成できませんでした");

            return id.Value;
        }
    }
}