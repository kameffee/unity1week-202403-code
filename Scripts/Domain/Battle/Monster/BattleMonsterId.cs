using System;

namespace Unity1week202403.Domain
{
    public readonly struct BattleMonsterId : IEquatable<BattleMonsterId>
    {
        public int Value { get; }
        public int TeamId => Value / 1_000;
        public int NumberId => Value % 1_000;

        public BattleMonsterId(int value, bool isAlly)
        {
            // 0-999: Ally, 1000-1999: Enemy
            var isAllyValue = (isAlly ? 0 : 1) * 1_000;
            Value = value + isAllyValue;
        }

        public bool Equals(BattleMonsterId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is BattleMonsterId other && Equals(other);
        public static bool operator ==(BattleMonsterId left, BattleMonsterId right) => left.Equals(right);
        public static bool operator !=(BattleMonsterId left, BattleMonsterId right) => !left.Equals(right);
        public override int GetHashCode() => Value;

        public static BattleMonsterId CreateAlly(int id) => new(id, true);
        public static BattleMonsterId CreateEnemy(int id) => new(id, false);
    }
}