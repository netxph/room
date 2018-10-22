using System;
using System.Collections.Generic;

namespace Room.Api.Controllers
{
    public class ReservationRepository
    {
        public void Create(Reservation reservation)
        {
            //saves to the database
        }

        public IEnumerable<Reservation> GetAll()
        {
            return new List<Reservation>()
            {
                new Reservation()
                {
                    From = new DateTime(2018, 12, 1, 12, 0, 0),
                    To = new DateTime(2018, 12, 1, 13, 0, 0),
                    User = new User()
                    {
                        Name = "Marc"
                    },
                    Location = new Location()
                    {
                        Name = "Kalesa"
                    }
                },
                new Reservation()
                {
                    From = new DateTime(2018, 12, 1, 13, 0, 0),
                    To = new DateTime(2018, 12, 1, 14, 0, 0),
                    User = new User()
                    {
                        Name = "James"
                    },
                    Location = new Location()
                    {
                        Name = "Anahaw"
                    }
                }
            };
        }
    }
}