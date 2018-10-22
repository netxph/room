using System.Collections.Generic;

namespace Room.Reservations.Core
{
    public interface IReservationRepository
    {
        void Create(Reservation reservation);
        IEnumerable<Reservation> GetAll();
        IEnumerable<Reservation> GetAllActive();
    }
}