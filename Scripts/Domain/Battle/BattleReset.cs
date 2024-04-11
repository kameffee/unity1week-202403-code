namespace Unity1week202403.Domain
{
    public readonly struct BattleReset
    {
        public bool Value { get; }

        public BattleReset(bool value)
        {
            Value = value;
        }
        
        public static BattleReset None => new(false);
    }
}