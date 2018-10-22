using System;

namespace Room.Reservations.Core
{
    public class Range : IEquatable<Range>
    {
        public Range(DateTime from, DateTime to)
        {
            if (from >= to)
            {
                throw new ArgumentOutOfRangeException(nameof(from));
            }

            From = from;
            To = to;
        }

        public DateTime From { get; }
        public DateTime To { get; }

        public bool Equals(Range other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return From.Equals(other.From) && To.Equals(other.To);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Range) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (From.GetHashCode() * 397) ^ To.GetHashCode();
            }
        }

        public bool Overlaps(Range range)
        {
            return (From >= range.From && From <= range.To) ||
                   (To >= range.From && To <= range.To);
        }
    }
}