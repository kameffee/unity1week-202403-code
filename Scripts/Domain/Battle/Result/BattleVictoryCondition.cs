using System.Linq;

namespace Unity1week202403.Domain
{
    public static class BattleVictoryCondition
    {
        public static bool IsFulfilled(BattleMonsterContainer battleMonsterContainer)
        {
            return battleMonsterContainer.GetEnemyBattleMonsters()
                .All(monster => monster.IsDead);
        }
    }
}