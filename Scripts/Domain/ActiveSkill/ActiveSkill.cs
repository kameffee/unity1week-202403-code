using Unity1week202403.Data;

namespace Unity1week202403.Domain
{
    public class ActiveSkill
    {
        public SkillType SkillType { get; }
        public SkillTargetType SkillTargetType { get; }
        public int Value { get; }
        public SkillRangeTargetType SkillRangeTargetType { get; }
        public KnockbackEffect KnockbackEffect { get; }
        public StanEffect StanEffect { get; }

        public ActiveSkill(SkillMasterData skillMasterData)
            : this(
                skillMasterData.SkillType,
                skillMasterData.SkillTargetType,
                skillMasterData.Value,
                skillMasterData.SkillRangeTargetType,
                new KnockbackEffect(skillMasterData.IsNknockback, skillMasterData.KnockbackPower),
                skillMasterData.AbnormalityEffect != null
                    ? new StanEffect(skillMasterData.AbnormalityEffect.StanTime)
                    : StanEffect.Empty)
        {
        }

        public ActiveSkill(
            SkillType skillType,
            SkillTargetType skillTargetType,
            int value,
            SkillRangeTargetType skillRangeTargetType,
            KnockbackEffect knockbackEffect = null,
            StanEffect stanEffect = null)
        {
            SkillType = skillType;
            SkillTargetType = skillTargetType;
            Value = value;
            SkillRangeTargetType = skillRangeTargetType;
            KnockbackEffect = knockbackEffect ?? KnockbackEffect.Default;
            StanEffect = stanEffect ?? StanEffect.Empty;
        }

        public static ActiveSkill DefaultSkill => new(
            SkillType.AttackDamage,
            SkillTargetType.Enemy,
            1,
            SkillRangeTargetType.Single,
            KnockbackEffect.Default,
            StanEffect.Empty);
    }
}