using UnityEngine;

namespace Unity1week202403.Domain
{
    public readonly struct Knockback
    {
        public Vector3 Direction { get; }
        public float Power { get; }

        public Knockback(Vector3 direction, float power)
        {
            Direction = direction;
            Power = power;
        }
    }
}