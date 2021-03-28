using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SignalRApp.Business.Utilities.Security.Jwt
{
    public class JwtResolver
    {
        private readonly IEnumerable<Claim> _claims;

        public JwtResolver(IEnumerable<Claim> claims)
        {
            _claims = claims;
        }

        public int GetUserId()
        {
            return int.Parse(_claims.First(x => x.Type == "user_id").Value);
        }

        public string GetClaimByType(string type)
        {
            return _claims.FirstOrDefault(c => c.Type == type)?.Value;
        }
    }
}