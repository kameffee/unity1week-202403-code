namespace Unity1week202403.Domain
{
    public class BattleResultCalculator
    {
        private readonly BattleMonsterContainer _battleMonsterContainer;

        public BattleResultCalculator(BattleMonsterContainer battleMonsterContainer)
        {
            _battleMonsterContainer = battleMonsterContainer;
        }

        public BattleResult Calculate()
        {
            if (BattleVictoryCondition.IsFulfilled(_battleMonsterContainer))
            {
                return BattleResult.Victory;
            }

            if (BattleDefeatCondition.IsFulfilled(_battleMonsterContainer))
            {
                return BattleResult.Defeat;
            }

            return BattleResult.None;
        }
    }
}