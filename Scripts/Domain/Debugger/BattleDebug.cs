using System;
using SRDebugger;
using VContainer.Unity;

namespace Unity1week202403.Domain
{
    public class BattleDebug : IInitializable, IDisposable
    {
        private const string CategoryName = "Battle";

        private readonly DynamicOptionContainer _container = new();
        private readonly BattleMonsterContainer _battleMonsterContainer;

        public BattleDebug(BattleMonsterContainer battleMonsterContainer)
        {
            _battleMonsterContainer = battleMonsterContainer;
        }

        public void Initialize()
        {
            _container.AddOption(OptionDefinition.FromMethod(
                name: "強制勝利",
                () =>
                {
                    // すべての敵モンスターを倒す
                    foreach (var enemyBattleMonster in _battleMonsterContainer.GetEnemyBattleMonsters())
                    {
                        enemyBattleMonster.Damaged(enemyBattleMonster.Hp.CurrentValue.Max);
                    }
                },
                category: CategoryName
            ));

            _container.AddOption(OptionDefinition.FromMethod(
                name: "強制敗北",
                () =>
                {
                    // すべての味方モンスターを倒す
                    foreach (var allyBattleMonster in _battleMonsterContainer.GetAllyBattleMonsters())
                    {
                        allyBattleMonster.Damaged(allyBattleMonster.Hp.CurrentValue.Max);
                    }
                },
                category: CategoryName
            ));

            SRDebug.Instance.AddOptionContainer(_container);
        }

        public void Dispose()
        {
            SRDebug.Instance?.RemoveOptionContainer(_container);
        }
    }
}