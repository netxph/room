using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Room.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post(Reservation reservation)
        {

            var validator = new ReservationValidator();
            var result = validator.Validate(reservation);

            if (result.IsValid)
            {
                try
                {
                    var security = new SecurityService();

                    if (security.HasAccess(User.Identity.Name))
                    {

                        var watch = Stopwatch.StartNew();

                        var repository = new ReservationRepository();

                        var reservations = 
                            repository
                                .GetAll()
                                .Where(r => r.To > DateTime.UtcNow).ToList();

                        if(reservations.Any(r => r.User.Name == User.Identity.Name 
                            && (reservation.From >= r.From && reservation.To <= r.To)))
                        {
                            throw new ArgumentOutOfRangeException("From", "Reservation is in conflict with your own module");
                        }

                        if (reservations.Any(r =>
                            (reservation.From >= r.From && reservation.To <= r.To) &&
                            r.Location.Name == reservation.Location.Name))
                        {
                            throw new ArgumentOutOfRangeException("From", "Reservation is in conflict with other reservation");
                        }

                        repository.Create(reservation);

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
