using System;

namespace Room.Reservations.Core
{
    public class ReservationCreatedEventArgs : EventArgs
    {

        public ReservationCreatedEventArgs(Reservation reservation)
        {
            Reservation = reservation ?? throw new ArgumentNullException(nameof(reservation));
        }

        public Reservation Reservation { get; }
    }
}