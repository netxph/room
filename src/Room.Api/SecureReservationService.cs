using System;
using System.Security;
using Room.Api.Controllers;
using Room.Reservations.Core;

namespace Room.Api
{
    public class SecureReservationService : IReservationService
    {

        private readonly IReservationService _service;
        private readonly ISecurityService _securityService;

        public SecureReservationService(IReservationService service, ISecurityService securityService)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _securityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
        }

        public void Reserve(Reservation reservation)
        {
            if (_securityService.HasAccess(CurrentSession.User.Name))
            {
                _service.Reserve(reservation);
            }
            else
            {
                throw new SecurityException();
            }

        }
    }
}
