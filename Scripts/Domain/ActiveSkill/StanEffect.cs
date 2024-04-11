using UnityEngine;

namespace Unity1week202403.Domain
{
    public class StanEffect
    {
        public bool Effectable => StanTime > 0;
        public float StanTime { get; }

        public StanEffect(float stanTime)
        {
            StanTime = Mathf.Max(0, stanTime);
        }

        public static StanEffect Empty => new(0);
    }
}