using System.Collections.Generic;

namespace Room.Api.Controllers
{
    public interface IReservationRepository
    {
        void Create(Reservation reservation);
        IEnumerable<Reservation> GetAll();
    }
}