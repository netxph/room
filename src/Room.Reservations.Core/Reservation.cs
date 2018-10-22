using System;

namespace Room.Reservations.Core
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

        public bool HasSameLocation(Location location)
        {
            return Location.Equals(location);
        }

        public bool Overlaps(Reservation reservation)
        {
            return Range.Overlaps(reservation.Range);
        }
    }
}