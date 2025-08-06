using DapperApiWithAuth.Models;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;


namespace DapperApiWithAuth.Service
{
    public interface ITokenService
    {
        // CreateToken method banaya gaya hai jiska kaam hoga JWT token generate karna
        string CreateToken(User user);
    }
    public class TokenServices : ITokenService
    {
        public readonly IConfiguration _config;
        public TokenServices(IConfiguration config)
        {
            _config = config;
        }
        public string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
            };

            // SymmetricSecurityKey = secret key jo JWT ko sign karegi
            // 
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["jwt:key"]));

            // SigningCredentials = JWT ko kaunsi algorithm se sign karna hai? Yahan HMAC-SHA512.
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var Token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
