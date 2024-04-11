using System.Linq;

namespace Unity1week202403.Domain
{
    public static class BattleDefeatCondition
    {
        public static bool IsFulfilled(BattleMonsterContainer battleMonsterContainer)
        {
            return battleMonsterContainer.GetAllyBattleMonsters()
                .All(monster => monster.IsDead);
        }
    }
}