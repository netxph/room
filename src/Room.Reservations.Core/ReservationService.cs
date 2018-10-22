using System;

namespace Room.Reservations.Core
{
    public class ReservationService : IReservationService
    {

        private readonly IReservationRepository _repository;

        public event EventHandler<ReservationCreatedEventArgs> Created; 

        public ReservationService(
            IReservationRepository repository
            )
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void Reserve(Reservation reservation)
        {

            var reservations =
                new Reservations(
                _repository
                    .GetAllActive());

            if (reservations.CanAdd(reservation))
            {
                _repository.Create(reservation);

                Created?.Invoke(this, new ReservationCreatedEventArgs(reservation));
            }

        }

    }
}
