using System;

namespace Unity1week202403.Structure
{
    public readonly struct MonsterId : IEquatable<MonsterId>
    {
        public int Value { get; }

        public MonsterId(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "0未満の値は指定できません");
            }

            Value = value;
        }

        public bool Equals(MonsterId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is MonsterId other && Equals(other);
        public static bool operator ==(MonsterId left, MonsterId right) => left.Equals(right);
        public static bool operator !=(MonsterId left, MonsterId right) => !left.Equals(right);
        public override int GetHashCode() => Value;
    }
}