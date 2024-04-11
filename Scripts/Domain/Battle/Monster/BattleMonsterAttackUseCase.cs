using System;
using Unity1week202403.Data;

namespace Unity1week202403.Domain
{
    public class BattleMonsterAttackUseCase
    {
        private readonly BattleMonsterContainer _battleMonsterContainer;
        private readonly KnockbackService _knockbackService;
        private readonly AbnormalityEffectCalculator _abnormalityEffectCalculator;

        public BattleMonsterAttackUseCase(
            BattleMonsterContainer battleMonsterContainer,
            KnockbackService knockbackService,
            AbnormalityEffectCalculator abnormalityEffectCalculator)
        {
            _battleMonsterContainer = battleMonsterContainer;
            _knockbackService = knockbackService;
            _abnormalityEffectCalculator = abnormalityEffectCalculator;
        }

        public void Calculate(BattleMonsterId attackerBattleMonsterId, SkillTarget skillTarget, ActiveSkill activeSkill)
        {
            var attackerMonster = _battleMonsterContainer.Get(attackerBattleMonsterId);
            switch (activeSkill.SkillType)
            {
                case SkillType.AttackDamage:
                    Attack(attackerMonster, skillTarget);
                    break;
                case SkillType.Heal:
                    Heal(attackerMonster, skillTarget, attackerMonster.ActiveSkill);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // スタン状態付与
            _abnormalityEffectCalculator.Apply(
                invoker: attackerMonster,
                skillTarget: skillTarget,
                CreateAbnormalityEffectState(attackerMonster, skillTarget)
            );
        }

        private IAbnormalityEffectState[] CreateAbnormalityEffectState(BattleMonster invoker, SkillTarget target)
        {
            return new IAbnormalityEffectState[]
            {
                new StanEffectState(invoker.ActiveSkill.StanEffect),
            };
        }

        private void Attack(BattleMonster attacker, SkillTarget skillTarget)
        {
            foreach (var monsterId in skillTarget.AllTargets)
            {
                var monster = _battleMonsterContainer.Get(monsterId);
                BattleMonsterAttackCalculator.Calculate(attacker, monster);
            }

            if (attacker.ActiveSkill.KnockbackEffect.IsKnockback)
            {
                _knockbackService.Apply(
                    attacker.BattleMonsterId,
                    skillTarget.AllTargets,
                    attacker.ActiveSkill.KnockbackEffect);
            }
        }

        private void Heal(BattleMonster invoker, SkillTarget skillTarget, ActiveSkill activeSkill)
        {
            foreach (var monsterId in skillTarget.AllTargets)
            {
                var monster = _battleMonsterContainer.Get(monsterId);
                BattleMonsterHealCalculator.Calculate(invoker, monster, activeSkill);
            }
        }
    }
}