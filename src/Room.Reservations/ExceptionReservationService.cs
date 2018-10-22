using System;
using Room.Reservations.Core;

namespace Room.Reservations
{
    public class ExceptionReservationService : IReservationService
    {

        private readonly IReservationService _service;

        public ExceptionReservationService(IReservationService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public void Reserve(Reservation reservation)
        {
            try
            {
                _service.Reserve(reservation);
            }
            catch (Exception e)
            {
                Logger.Log(e.Message);
                throw;
            }
        }
    }
}
