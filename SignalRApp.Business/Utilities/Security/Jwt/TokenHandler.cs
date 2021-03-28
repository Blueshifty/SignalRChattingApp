using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SignalRApp.Data.Models;

namespace SignalRApp.Business.Utilities.Security.Jwt
{
    public class TokenHandler : ITokenHandler
    {
        private IConfiguration Configuration { get; set; }

        public TokenHandler(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public Token CreateAccessToken(User user)
        {
            var tokenInstance = new Token();

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:SecurityKey"]));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            tokenInstance.Expiration = DateTime.Now.AddHours(1);

            var securityToken = new JwtSecurityToken(
                issuer: Configuration["Token:Issuer"],
                audience: Configuration["Token:Audience"],
                claims: SetClaims(user),
                expires: tokenInstance.Expiration,
                notBefore: DateTime.Now,
                signingCredentials: signingCredentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();

            tokenInstance.AccessToken = tokenHandler.WriteToken(securityToken);

            tokenInstance.RefreshToken = CreateRefreshToken();

            return tokenInstance;
        }

        private IEnumerable<Claim> SetClaims(User user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            return claims;
        }

        private static string CreateRefreshToken()
        {
            var number = new byte[32];
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(number);
                return Convert.ToBase64String(number);
            }
        }
    }
}