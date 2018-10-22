namespace Room.Api
{
    public interface ISecurityService
    {
        bool HasAccess(string user);
    }
}