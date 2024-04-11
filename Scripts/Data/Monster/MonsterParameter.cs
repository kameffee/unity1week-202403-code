using System;
using UnityEngine;

namespace Unity1week202403.Data
{
    [Serializable]
    public class MonsterParameter
    {
        public int Hp => _hp;
        public int AttackPower => _attackPower;
        public float AttackRange => _attackRange;
        public float PreAttackTime => _preAttackFrame/60f;
        public float PostAttackTime => (_attackEndFrame - _preAttackFrame)/60f;
        public float MoveSpeed => _moveSpeed;

        [SerializeField]
        private int _hp = 1;

        [SerializeField]
        private int _attackPower = 1;

        [SerializeField]
        private float _attackRange = 1;

        [SerializeField]
        private int _preAttackFrame = 60;

        [SerializeField]
        private int _attackEndFrame = 120;
        
        [SerializeField]
        private float _moveSpeed = 1;
    }
}