using System;

namespace Unity1week202403.Structure
{
    public readonly struct StageId : IEquatable<StageId>, IComparable<StageId>

    {
        public int Value { get; }

        public StageId(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "0未満の値は指定できません");
            }

            Value = value;
        }

        public bool Equals(StageId other) => Value == other.Value;
        public int CompareTo(StageId other) => Value.CompareTo(other.Value);
        public override bool Equals(object obj) => obj is StageId other && Equals(other);
        public static bool operator ==(StageId left, StageId right) => left.Equals(right);
        public static bool operator !=(StageId left, StageId right) => !left.Equals(right);
        public override int GetHashCode() => Value;
        public override string ToString() => Value.ToString();
    }
}