namespace Unity1week202403.Domain
{
    public class AbnormalityEffectCalculator
    {
        private readonly BattleMonsterContainer _battleMonsterContainer;
        
        public AbnormalityEffectCalculator(BattleMonsterContainer battleMonsterContainer)
        {
            _battleMonsterContainer = battleMonsterContainer;
        }

        public void Apply(
            BattleMonster invoker,
            SkillTarget skillTarget,
            IAbnormalityEffectState[] abnormalityEffectStateCollection)
        {
            foreach (var battleMonsterId in skillTarget.AllTargets)
            {
                var target = _battleMonsterContainer.Get(battleMonsterId);
                target.AddAbnormality(abnormalityEffectStateCollection);
            }
        }
    }
}