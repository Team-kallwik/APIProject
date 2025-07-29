using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Text;
using Dapper;
using Dapper_Api_With_Token_Authentication.Model;

namespace Dapper_Api_With_Token_Authentication.Controllers
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

        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            _logger.LogInformation("Register API called with Username: {Username}", newUser.Username);

            try
            {
                var json = JsonSerializer.Serialize(new[] { newUser });

                using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
                var result = connection.QueryFirstOrDefault<string>(
                    "RegisterUserFromJson",
                    new { json },
                    commandType: CommandType.StoredProcedure
                );

                return result switch
                {
                    "Registered" => Ok(new { message = "User registered successfully" }),
                    "UserAlreadyExists" => Conflict("Username already exists"),
                    _ => StatusCode(500, "Unexpected error during registration")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during registration for Username: {Username}", newUser.Username);
                return StatusCode(500, new { message = "Error occurred while registering", error = ex.Message });
            }
        }

        [HttpPost("login")]
        public IActionResult Login(User login)
        {
            _logger.LogInformation("Login attempt for Username: {Username}", login.Username);

            try
            {
                var json = JsonSerializer.Serialize(new[] { login });

                using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

                var result = connection.QueryFirstOrDefault<string>(
                    "LoginUserFromJson",
                    new { json },
                    commandType: CommandType.StoredProcedure
                );

                if (result == "Success")
                {
                    _logger.LogInformation("Login successful for Username: {Username}", login.Username);
                    var token = GenerateToken(login.Username);
                    return Ok(new { token });
                }

                _logger.LogWarning("Invalid login attempt for Username: {Username}", login.Username);
                return Unauthorized("Invalid credentials");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for Username: {Username}", login.Username);
                return StatusCode(500, new { message = "An error occurred while logging in", error = ex.Message });
            }
        }

        private string GenerateToken(string username)
        {
            _logger.LogInformation("Generating JWT token for Username: {Username}", username);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
