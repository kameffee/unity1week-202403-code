using Unity1week202403.Data;

namespace Unity1week202403.Domain
{
    public class StanEffectState : IAbnormalityEffectState
    {
        public AbnormalityType Type => AbnormalityType.Stan;

        public bool Effectable { get; private set; } = true;

        private readonly StanEffect _stanEffect;
        private float _endTime;

        public StanEffectState(StanEffect stanEffect)
        {
            _stanEffect = stanEffect;
        }

        public void Start(float startTime)
        {
            _endTime = startTime + _stanEffect.StanTime;
        }

        public void UpdateTime(float currentTime)
        {
            if (currentTime >= _endTime)
            {
                Effectable = false;
            }
        }
    }
}