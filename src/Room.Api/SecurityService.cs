namespace Room.Api
{
    public class SecurityService : ISecurityService
    {
        public bool HasAccess(string user)
        {
            return true;
        }
    }
}