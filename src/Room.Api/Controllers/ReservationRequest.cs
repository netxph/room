using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Room.Api.Controllers
{
    public class ReservationRequest
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Location { get; set; }
    }
}
