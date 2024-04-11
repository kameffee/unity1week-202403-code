using UnityEngine;

namespace Unity1week202403.Structure
{
    public readonly struct AudioVolume
    {
        public static AudioVolume Zero => new(0);
        public static AudioVolume Max => new(1);

        public float Value => _value;

        private readonly float _value;

        public AudioVolume(float volume)
        {
            _value = Mathf.Clamp01(volume);
        }

        public bool IsMax() => _value >= Max.Value;

        public bool IsZero() => _value <= Zero.Value;
    }
}