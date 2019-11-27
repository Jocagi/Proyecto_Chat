using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using API.Services;

namespace API.Services
{
    public class TokenProvider
    {
        private readonly SymmetricSecurityKey SIGNING_KEY;

        public TokenProvider(ITokenSettings settings)
        {
            SIGNING_KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecretKey));
        }
        public object GenerateToken(string username)
        {
            var token = new JwtSecurityToken(
                claims: new Claim[] { new Claim(ClaimTypes.Name, username) },
                notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                expires: new DateTimeOffset(DateTime.Now.AddMinutes(60)).DateTime,
                signingCredentials: new SigningCredentials(SIGNING_KEY,
                    SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
