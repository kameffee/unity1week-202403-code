using System.Linq;
using Unity1week202403.Structure;
using UnityEngine.Assertions;

namespace Unity1week202403.Domain
{
    public class CreateAllyBattleMonsterUseCase
    {
        private readonly BattleMonsterFactory _battleMonsterFactory;
        private readonly BattleMonsterContainer _battleMonsterContainer;

        public CreateAllyBattleMonsterUseCase(
            BattleMonsterFactory battleMonsterFactory,
            BattleMonsterContainer battleMonsterContainer)
        {
            _battleMonsterFactory = battleMonsterFactory;
            _battleMonsterContainer = battleMonsterContainer;
        }

        public BattleMonster Create(MonsterId monsterId)
        {
            var uniqueBattleMonsterId = CreateUniqueBattleMonsterId();
            var monster = _battleMonsterFactory.Create(uniqueBattleMonsterId, monsterId, isAlly: true);
            return monster;
        }

        private BattleMonsterId CreateUniqueBattleMonsterId()
        {
            var ids = _battleMonsterContainer.GetAllyIds().ToArray();
            BattleMonsterId? id = null;

            //  重複しないIDを生成する
            for (int i = 0; i < 999; i++)
            {
                // 候補
                var candidate = BattleMonsterId.CreateAlly(i);
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