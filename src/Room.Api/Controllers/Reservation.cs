using System;

namespace Room.Api.Controllers
{
    public class Reservation
    {
        public Reservation(User user, Location location, Range range)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
            Location = location ?? throw new ArgumentNullException(nameof(location));
            Range = range ?? throw new ArgumentNullException(nameof(range));
        }

        public User User { get; }
        public Location Location { get; }
        public Range Range { get; }
    }

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
    }

    public class Location
    {
        public Location(int id, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;

            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            ID = id;
        }

        public Location(string name)
            : this(0, name)
        {
            
        }

        public int ID { get; }
        public string Name { get; }
    }

    public class User
    {

        public User(int id, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;

            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            ID = id;
        }

        public User(string name)
            :this(0, name)
        {
            
        }

        public int ID { get; }
        public string Name { get; }
    }
}