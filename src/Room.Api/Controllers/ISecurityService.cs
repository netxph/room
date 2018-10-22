namespace Room.Api.Controllers
{
    public interface ISecurityService
    {
        bool HasAccess(string user);
    }
}