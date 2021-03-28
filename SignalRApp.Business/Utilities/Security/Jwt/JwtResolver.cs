using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace SignalRApp.Business.Utilities.Security.Jwt
{
    public class JwtResolver
    {
        private readonly JwtSecurityTokenHandler _handler;
        private readonly JwtSecurityToken _jwtToken;
        private readonly string _token;


        public JwtResolver(string token)
        {
            _handler = new JwtSecurityTokenHandler();
            _token = token;
            _jwtToken = _handler.ReadToken(_token) as JwtSecurityToken;
        }

        public string GetClaim(string claimName)
        {
            string claim = null;
            if (_jwtToken != null) claim = _jwtToken.Claims.First(c => c.Type == claimName).Value;

            return claim;
        }
    }
}