using SignalRApp.Data.Models;

namespace SignalRApp.Business.Utilities.Security.Jwt
{
    public interface ITokenHandler
    {
        Token CreateAccessToken(User user);
    }
}