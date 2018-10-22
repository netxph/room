using System;
using FluentValidation;
using Room.Reservations.Core;

namespace Room.Reservations
{
    public class ValidateReservationService : IReservationService
    {
        private readonly IReservationService _service;
        private readonly AbstractValidator<Reservation> _validator;

        public ValidateReservationService(IReservationService service, AbstractValidator<Reservation> validator)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public void Reserve(Reservation reservation)
        {
            var result = _validator.Validate(reservation);

            if (result.IsValid)
            {
                _service.Reserve(reservation);
            }
            else
            {
                throw new ArgumentException("Bad request", nameof(reservation));
            }

        }
    }
}
