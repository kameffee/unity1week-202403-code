using UnityEngine;

namespace Unity1week202403.Data
{
    [CreateAssetMenu(menuName = "Skill/MasterData", fileName = "SkillMasterData", order = 0)]
    public class SkillMasterData : ScriptableObject
    {
        public SkillType SkillType => _skillType;
        public SkillTargetType SkillTargetType => _skillTargetType;
        public int Value => _value;
        public SkillRangeTargetType SkillRangeTargetType => _skillRangeTargetType;
        public bool IsNknockback => _isNknockback;
        public float KnockbackPower => _knockbackPower;
        public AbnormalityEffectMasterData AbnormalityEffect => _abnormalityEffect;

        [SerializeField]
        private SkillType _skillType = SkillType.AttackDamage;

        [SerializeField]
        private SkillTargetType _skillTargetType;

        [SerializeField]
        private int _value;

        [SerializeField]
        private SkillRangeTargetType _skillRangeTargetType;

        [Header("Knockback")]
        [SerializeField]
        private bool _isNknockback;

        [SerializeField]
        private float _knockbackPower;

        [Header("状態異常効果付与")]
        [SerializeField]
        private AbnormalityEffectMasterData _abnormalityEffect;
        
        [TextArea(3, 10)]
        [SerializeField]
        private string _description;
    }
}