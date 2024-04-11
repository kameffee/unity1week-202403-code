using UnityEngine;

namespace Unity1week202403.Structure
{
    public readonly struct Heal
    {
        public int BeforeHp { get; }
        public int AfterHp { get; }
        public int Value { get; }
        public int ActualValue { get; }

        public Heal(int beforeHp, int afterHp, int value, int actualValue)
        {
            BeforeHp = beforeHp;
            AfterHp = afterHp;
            Value = value;
            ActualValue = actualValue;
        }

        public static Heal Create(int maxHp, int beforeHp, int value)
        {
            var afterHp = Mathf.Min(maxHp, beforeHp + value);
            return new Heal(beforeHp, afterHp, value, afterHp - beforeHp);
        }

        public override string ToString()
        {
            return $"BeforeHp: {BeforeHp}, AfterHp: {AfterHp}, Value: {Value}, ActualValue: {ActualValue}";
        }
    }
}