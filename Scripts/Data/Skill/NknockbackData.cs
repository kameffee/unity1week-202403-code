using System;
using UnityEngine;

namespace Unity1week202403.Data
{
    [Serializable]
    public class NknockbackData
    {
        public bool IsKnockback => _isNknockback;
        public float KnockbackPower => _knockbackPower;

        [SerializeField]
        private bool _isNknockback;

        [SerializeField]
        private float _knockbackPower;

        public NknockbackData(bool isNknockback, float knockbackPower)
        {
            _isNknockback = isNknockback;
            _knockbackPower = knockbackPower;
        }

        public static NknockbackData Default => new(false, 0);
    }
}