using System;
using System.Collections.Generic;

namespace Unity1week202403.Structure
{
    public readonly struct Cost : IEquatable<Cost>, IEqualityComparer<Cost>
    {
        public int Value { get; }

        public Cost(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "Costは0以上である必要があります");
            }

            Value = value;
        }

        public bool Equals(Cost other) => Value == other.Value;
        public override bool Equals(object obj) => obj is Cost other && Equals(other);
        public override int GetHashCode() => Value;

        public static bool operator ==(Cost left, Cost right) => left.Equals(right);
        public static bool operator !=(Cost left, Cost right) => !left.Equals(right);

        public static bool operator <(Cost left, Cost right) => left.Value < right.Value;
        public static bool operator >(Cost left, Cost right) => left.Value > right.Value;
        public static bool operator <=(Cost left, Cost right) => left.Value <= right.Value;
        public static bool operator >=(Cost left, Cost right) => left.Value >= right.Value;

        public bool Equals(Cost x, Cost y) => x.Value == y.Value;
        public int GetHashCode(Cost obj) => obj.Value;
    }
}