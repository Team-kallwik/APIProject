using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DapperAuthApi.Services
{
    public class JwtAuthManager : IJwtAuthManager
    {
        private readonly IConfiguration _config;

        public JwtAuthManager(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(string username)
        {
            try
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, username)
                };

                var keyString = _config["Jwt:Key"];
                var issuer = _config["Jwt:Issuer"];
                var audience = _config["Jwt:Audience"];

                if (string.IsNullOrEmpty(keyString) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
                {
                    throw new Exception("JWT configuration values are missing.");
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                // You can log the error here if needed
                throw new Exception("Error generating JWT token.", ex);
            }
        }
    }
}