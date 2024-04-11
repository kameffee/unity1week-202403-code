using System;

namespace Unity1week202403.Structure
{
    public readonly struct Hp : IEquatable<Hp>
    {
        public int Value { get; }
        public int Max { get; }
        public bool IsZero => Value <= 0;
        public float NormalizeValue { get; }
        public int DecreaseValue => Max - Value;

        public Hp(int value, int max)
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "valueが0未満です");
            if (max < 0) throw new ArgumentOutOfRangeException(nameof(max), "maxが0未満です");
            if (value > max) throw new ArgumentException("valueがmaxを超えています");

            Value = value;
            Max = max;
            NormalizeValue = (float)Value / Max;
        }

        public Hp Add(int value) => new(Math.Min(Value + value, Max), Max);
        public Hp Subtract(int value) => new(Math.Max(Value - value, 0), Max);

        public static Hp Create(int value, int max) => new(value, max);
        public static Hp Full(int max) => new(max, max);

        public bool Equals(Hp other) => Value == other.Value && Max == other.Max;
        public override bool Equals(object obj) => obj is Hp other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Value, Max);
    }
}