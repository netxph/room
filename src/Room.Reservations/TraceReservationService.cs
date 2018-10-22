using System;
using System.Diagnostics;
using Room.Reservations.Core;

namespace Room.Reservations
{
    public class TraceReservationService : IReservationService
    {

        private readonly IReservationService _service;

        public TraceReservationService(IReservationService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public void Reserve(Reservation reservation)
        {
            var watch = Stopwatch.StartNew();
            _service.Reserve(reservation);

            watch.Stop();

            Logger.Log($"Elapsed: {watch.ElapsedMilliseconds}ms");
        }
    }
}
