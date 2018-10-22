namespace Room.Api.Controllers
{
    public class SecurityService : ISecurityService
    {
        public bool HasAccess(string user)
        {
            return true;
        }
    }
}