namespace Unity1week202403.Domain
{
    public class BattleMonsterHealCalculator
    {
        public static void Calculate(BattleMonster invoker, BattleMonster target, ActiveSkill activeSkill)
        {
            if (invoker.IsDead || target.IsDead)
            {
                return;
            }

            target.Healed((int)activeSkill.Value);
        }
    }
}