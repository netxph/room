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

        private readonly ISecurityService _securityService;
        private readonly AbstractValidator<Reservation> _validator;
        private readonly IReservationRepository _repository;

        public ReservationService(
            ISecurityService securityService,
            IReservationRepository repository,
            AbstractValidator<Reservation> validator)
        {
            _securityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void Reserve(Reservation reservation)
        {
            var result = _validator.Validate(reservation);

            if (result.IsValid)
            {
                try
                {
                    if (_securityService.HasAccess(CurrentSession.User.Name))
                    {
                        var watch = Stopwatch.StartNew();

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

                        watch.Stop();

                        Logger.Log($"Elapsed: {watch.ElapsedMilliseconds}ms");
                    }

                    throw new SecurityException();
                }
                catch (Exception ex)
                {
                    Logger.Log($"[ERROR]: {ex.Message}");
                    throw;
                }
            }
            else
            {
                throw new ArgumentException("Validation failed", nameof(reservation));
            }
        }

    }
}
