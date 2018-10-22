using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using FluentValidation;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Room.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {

        private readonly ISecurityService _securityService;
        private readonly AbstractValidator<Reservation> _validator;
        private readonly IReservationRepository _repository;

        public ReservationsController(
            ISecurityService securityService, 
            IReservationRepository repository, 
            AbstractValidator<Reservation> validator)
        {
            _securityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpPost]
        public IActionResult Post(ReservationRequest request)
        {

            var reservation = new Reservation(new User(CurrentSession.User.Name), new Location(request.Location),
                new Range(request.From, request.To));

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

                        if(reservations.Any(r => r.User.Name == User.Identity.Name 
                            && (reservation.Range.From >= r.Range.From && reservation.Range.To <= r.Range.To)))
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
                            var content = new StringContent(JsonConvert.SerializeObject(reservation), Encoding.UTF8, "application/json");
                            client.PostAsync("http://localhost:5000/api/analytics",
                                content)
                                .GetAwaiter()
                                .GetResult();
                        }

                        watch.Stop();

                        Logger.Log($"Elapsed: {watch.ElapsedMilliseconds}ms");

                        return NoContent();
                    }

                    return Unauthorized();
                }
                catch (Exception ex)
                {
                    Logger.Log($"[ERROR]: {ex.Message}");
                    return StatusCode(500);
                }
            }
            else
            {
                return BadRequest();
            }
        }
            

    }
}
