using DapperWebApiWIthAuthentication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;

namespace DapperWebApiWIthAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration config, ILogger<AuthController> logger)
        {
            _config = config;
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginEmployee loginEmployee)
        {
            try
            {
                _logger.LogInformation("Login attempt for user: {Username}", loginEmployee.Username);

                if (loginEmployee.Username == "admin" && loginEmployee.Password == "123")
                {
                    var token = GenerateToken(loginEmployee.Username);
                    _logger.LogInformation("JWT generated successfully for user: {Username}", loginEmployee.Username);
                    return Ok(new { token });
                }

                _logger.LogWarning("Invalid credentials for user: {Username}", loginEmployee.Username);
                return Unauthorized("Invalid credentials");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login for user: {Username}", loginEmployee.Username);
                return StatusCode(500, "Internal server error: {ex.Message}");
            }
        }

        private string GenerateToken(string username)
        {
            try
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, username)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating JWT token for user: {Username}", username);
                throw new Exception("Error generating JWT token: {ex.Message}");
            }
        }
    }
}
