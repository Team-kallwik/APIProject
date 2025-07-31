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
using Dapper_Api_With_Token_Authentication.Helpers;

namespace Dapper_Api_With_Token_Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;
        private readonly AesEncryptionHelper _aesHelper;

        public AuthController(IConfiguration config, ILogger<AuthController> logger, AesEncryptionHelper aesHelper)
        {
            _config = config;
            _logger = logger;
            _aesHelper = aesHelper;
        }

        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            _logger.LogInformation("Register API called with Username: {Username}", newUser.Username);

            try
            {
                // Encrypt password before saving
                newUser.Password = _aesHelper.Encrypt(newUser.Password);

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
                string encryptedPassword = _aesHelper.Encrypt(login.Password);

                // First try with encrypted password
                var jsonEncrypted = JsonSerializer.Serialize(new[] { new { login.Username, Password = encryptedPassword } });

                using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
                var result = connection.QueryFirstOrDefault<string>(
                    "LoginUserFromJson",
                    new { json = jsonEncrypted },
                    commandType: CommandType.StoredProcedure
                );

                if (result == "Success")
                {
                    _logger.LogInformation("Login successful (encrypted) for Username: {Username}", login.Username);
                    var token = GenerateToken(login.Username);
                    return Ok(new { token });
                }

                //Try with plain password (old users)
                var jsonPlain = JsonSerializer.Serialize(new[] { login });
                result = connection.QueryFirstOrDefault<string>(
                    "LoginUserFromJson",
                    new { json = jsonPlain },
                    commandType: CommandType.StoredProcedure
                );

                if (result == "Success")
                {
                    _logger.LogInformation("Login successful (plain) for Username: {Username}", login.Username);
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
