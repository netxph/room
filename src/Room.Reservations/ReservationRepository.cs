using System;
using System.Collections.Generic;
using Room.Reservations.Core;

namespace Room.Reservations
{
    public class ReservationRepository : IReservationRepository
    {
        public void Create(Reservation reservation)
        {
            //saves to the database
        }

        public IEnumerable<Reservation> GetAll()
        {
            return new List<Reservation>()
            {
                new Reservation(
                    new User("Marc"),
                    new Location("Anahaw"), 
                    new Range(   
                        new DateTime(2018, 12, 1, 12, 0, 0),
                        new DateTime(2018, 12, 1, 13, 0, 0)))
            };
        }

        public IEnumerable<Reservation> GetAllActive()
        {
            return GetAll();
        }
    }
}