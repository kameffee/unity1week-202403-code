using System;
using R3;
using Unity1week202403.Data;
using Unity1week202403.Structure;
using UnityEngine;

namespace Unity1week202403.Domain
{
    public class BattleMonster : IEquatable<BattleMonster>
    {
        public BattleMonsterId BattleMonsterId { get; }
        public MonsterId MonsterId { get; }
        public Cost Cost { get; }
        public bool IsAlly { get; }
        public bool IsEnemy => !IsAlly;
        public bool IsDead => _hp.Value.IsZero;
        public MonsterParameter Parameter { get; }
        public ActiveSkill ActiveSkill { get; }
        public int ZPosition { get; }
        public ReadOnlyReactiveProperty<Hp> Hp => _hp;
        public Observable<Unit> OnDead => _onDead;
        public Observable<Unit> OnDamaged => _onDamaged;
        public Observable<Heal> OnHeal => _onHeal;
        public Observable<Knockback> OnKnockback => _onKnockback;

        public Observable<AbnormalityTypeCollection> OnUpdateAbnormalityEffects
            => _abnormalityEffectCollection.OnUpdateAbnormalityType;

        private readonly AbnormalityEffectCollection _abnormalityEffectCollection = new();
        private readonly ReactiveProperty<Hp> _hp;
        private readonly Subject<Unit> _onDead = new();
        private readonly Subject<Unit> _onDamaged = new();
        private readonly Subject<Heal> _onHeal = new();
        private readonly Subject<Knockback> _onKnockback = new();

        public BattleMonster(
            BattleMonsterId battleMonsterId,
            MonsterId monsterId,
            bool isAlly,
            Cost cost,
            MonsterParameter parameter,
            ActiveSkill activeSkill,
            int zPosition)
        {
            BattleMonsterId = battleMonsterId;
            MonsterId = monsterId;
            IsAlly = isAlly;
            Cost = cost;
            Parameter = parameter;
            ActiveSkill = activeSkill;
            _hp = new ReactiveProperty<Hp>(Structure.Hp.Full(parameter.Hp));
            ZPosition = zPosition;
        }

        public void Damaged(int damage)
        {
            _hp.Value = _hp.Value.Subtract(damage);
            _onDamaged.OnNext(Unit.Default);
            if (IsDead)
            {
                _onDead.OnNext(Unit.Default);
            }
        }

        public void Healed(int value)
        {
            var heal = Heal.Create(
                maxHp: Parameter.Hp,
                beforeHp: _hp.Value.Value,
                value: value);

            var afterHp = _hp.Value.Add(value);
            _onHeal.OnNext(heal);
            _hp.Value = afterHp;
        }

        public bool Equals(BattleMonster other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return BattleMonsterId.Equals(other.BattleMonsterId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BattleMonster)obj);
        }

        public override int GetHashCode()
        {
            return BattleMonsterId.GetHashCode();
        }

        public bool IsEnemyTo(BattleMonster target)
        {
            return BattleMonsterId.TeamId != target.BattleMonsterId.TeamId;
        }

        public bool IsAllyTo(BattleMonster monster)
        {
            return BattleMonsterId.TeamId == monster.BattleMonsterId.TeamId;
        }

        public void Knockback(Knockback knockback)
        {
            _onKnockback.OnNext(knockback);
        }

        public void AddAbnormality(params IAbnormalityEffectState[] stanEffectState)
        {
            _abnormalityEffectCollection.Add(stanEffectState);
        }

        public void UpdateTime(float currentTime)
        {
            _abnormalityEffectCollection.Update(currentTime);
        }

        public bool IsMovable()
        {
            return _abnormalityEffectCollection.IsMovable();
        }

        public bool IsAttackable()
        {
            return _abnormalityEffectCollection.IsAttackable();
        }
    }
}