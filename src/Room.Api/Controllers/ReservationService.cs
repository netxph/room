using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Newtonsoft.Json;

namespace Room.Api.Controllers
{
    public class ReservationService : IReservationService
    {

        private readonly IReservationRepository _repository;

        public ReservationService(
            IReservationRepository repository
            )
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void Reserve(Reservation reservation)
        {

            var reservations =
                _repository
                    .GetAll()
                    .Where(r => r.Range.To > DateTime.UtcNow).ToList();

            if (reservations.Any(r => r.User.Name == reservation.User.Name
                                      && (reservation.Range.From >= r.Range.From &&
                                          reservation.Range.To <= r.Range.To)))
            {
                throw new ArgumentOutOfRangeException("From", "Reservation is in conflict with your own module");
            }

            if (reservations.Any(r =>
                (reservation.Range.From >= r.Range.From && reservation.Range.To <= r.Range.To) &&
                r.Location.Name == reservation.Location.Name))
            {
                throw new ArgumentOutOfRangeException("From", "Reservation is in conflict with other reservation");
            }

            _repository.Create(reservation);

            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(reservation), Encoding.UTF8,
                    "application/json");
                client.PostAsync("http://localhost:5000/api/analytics",
                        content)
                    .GetAwaiter()
                    .GetResult();
            }

        }

    }
}
