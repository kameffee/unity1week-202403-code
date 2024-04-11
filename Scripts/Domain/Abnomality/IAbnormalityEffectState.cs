using Unity1week202403.Data;

namespace Unity1week202403.Domain
{
    public interface IAbnormalityEffectState
    {
        AbnormalityType Type { get; }

        bool Effectable { get; }

        void Start(float startTime);

        void UpdateTime(float currentTime);
    }
}