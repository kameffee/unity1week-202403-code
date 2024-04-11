using UnityEngine;

namespace Unity1week202403.Data
{
    [CreateAssetMenu(menuName = "Skill/AbnormalityEffect", fileName = "AbnormalityEffect_", order = 0)]
    public class AbnormalityEffectMasterData : ScriptableObject
    {
        public AbnormalityType AbnormalityType => _abnormalityType;
        public float StanTime => _stanTime;

        [Header("スタン効果")]
        [SerializeField]
        private AbnormalityType _abnormalityType;

        [SerializeField]
        private float _stanTime = 0;
    }
}