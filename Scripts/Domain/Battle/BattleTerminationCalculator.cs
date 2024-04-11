namespace Unity1week202403.Domain
{
    public class BattleTerminationCalculator
    {
        private readonly BattleMonsterContainer _battleMonsterContainer;

        public BattleTerminationCalculator(BattleMonsterContainer battleMonsterContainer)
        {
            _battleMonsterContainer = battleMonsterContainer;
        }

        public bool IsBattleTerminated()
        {
            return IsVictory() || IsDefeat();
        }

        private bool IsVictory()
        {
            return BattleVictoryCondition.IsFulfilled(_battleMonsterContainer);
        }

        private bool IsDefeat()
        {
            return BattleDefeatCondition.IsFulfilled(_battleMonsterContainer);
        }
    }
}