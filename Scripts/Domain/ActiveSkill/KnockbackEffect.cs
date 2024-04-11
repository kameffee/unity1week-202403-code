namespace Unity1week202403.Domain
{
    public class KnockbackEffect
    {
        public bool IsKnockback { get; }
        public float KnockbackPower { get; }

        public KnockbackEffect(bool isKnockback, float knockbackPower)
        {
            IsKnockback = isKnockback;
            KnockbackPower = knockbackPower;
        }

        public static KnockbackEffect Default => new(false, 0);
    }
}