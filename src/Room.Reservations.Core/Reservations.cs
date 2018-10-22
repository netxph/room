using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Room.Reservations.Core
{
    public class Reservations : IEnumerable<Reservation>
    {

        private readonly List<Reservation> _list;

        public Reservations(IEnumerable<Reservation> reservations)
        {
            _list = new List<Reservation>();

            if (reservations == null)
            {
                throw new ArgumentNullException(nameof(reservations));
            }

            _list.AddRange(reservations);
        }

        public IEnumerator<Reservation> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _list).GetEnumerator();
        }

        public bool CanAdd(Reservation reservation)
        {
            return !HasRoomConflict(reservation) && !HasUserConflict(reservation);
        }

        public bool HasRoomConflict(Reservation reservation)
        {
            if (_list.Any(r =>
                r.Overlaps(reservation) &&
                r.HasSameLocation(reservation.Location)))
            {
                return true;
            }

            return false;
        }

        public bool HasUserConflict(Reservation reservation)
        {
            if (_list.Any(r => r.User.Name == reservation.User.Name
                               && (reservation.Range.From >= r.Range.From &&
                                   reservation.Range.To <= r.Range.To)))
            {
                return true;
            }

            return false;
        }
    }
}