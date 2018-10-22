using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Security;
using System.Text;
using FluentValidation;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Room.Reservations.Core;

namespace Room.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {

        private readonly IReservationService _service;

        public ReservationsController(IReservationService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Post(ReservationRequest request)
        {
            var reservation = new Reservation(new User(CurrentSession.User.Name), new Location(request.Location),
                new Range(request.From, request.To));

            try
            {
                _service.Reserve(reservation);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (SecurityException)
            {
                return Unauthorized();
            }

        }
        
    }
}
