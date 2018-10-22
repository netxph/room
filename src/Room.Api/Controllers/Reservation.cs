using System;

namespace Room.Api.Controllers
{
    public class Reservation
    {
        public DateTime To { get; set; }
        public User User { get; set; }
        public DateTime From { get; set; }
        public Location Location { get; set; }
    }

    public class Location
    {
        public string Name { get; set; }
    }

    public class User
    {
        public string Name { get; set; }
    }
}