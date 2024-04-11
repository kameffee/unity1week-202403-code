using System;
using Unity1week202403.Data;

namespace Unity1week202403.Domain
{
    public readonly struct AbnormalityTypeCollection : IEquatable<AbnormalityTypeCollection>
    {
        public AbnormalityType[] Types { get; }

        public AbnormalityTypeCollection(AbnormalityType[] types)
        {
            Types = types ?? Array.Empty<AbnormalityType>();
        }

        public bool Contains(AbnormalityType type)
        {
            foreach (var abnormalityType in Types)
            {
                if (abnormalityType == type)
                {
                    return true;
                }
            }

            return false;
        }

        public bool Equals(AbnormalityTypeCollection other) => Equals(Types, other.Types);
        public override bool Equals(object obj) => obj is AbnormalityTypeCollection other && Equals(other);
        public override int GetHashCode() => (Types != null ? Types.GetHashCode() : 0);
        
        public static AbnormalityTypeCollection Empty => new(Array.Empty<AbnormalityType>());
    }
}