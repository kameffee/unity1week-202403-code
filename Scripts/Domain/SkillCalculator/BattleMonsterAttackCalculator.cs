namespace Unity1week202403.Domain
{
    public static class BattleMonsterAttackCalculator
    {
        public static void Calculate(BattleMonster attacker, BattleMonster target)
        {
            if (attacker.IsDead || target.IsDead)
            {
                return;
            }

            target.Damaged(attacker.Parameter.AttackPower);
        }
    }
}