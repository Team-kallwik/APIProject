using Dapper;
using DapperWebApiWIthAuthentication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DapperWebApiWIthAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DapperContext _context;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _config;
        private readonly EncryptionHelper _encryptionHelper;


        public AuthController(DapperContext context, ILogger<AuthController> logger, IConfiguration config, EncryptionHelper encryptionHelper)
        {
            _config = config;
            _context = context;
            _logger = logger;
            _encryptionHelper = encryptionHelper;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterEmployee user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Password))
            {
                _logger.LogWarning("Invalid registration input received.");
                return BadRequest("Invalid input.");
            }

            try
            {
                _logger.LogInformation("Encrypting password for user: {UserName}", user.UserName);
                var encryptedPassword = _encryptionHelper.Encrypt(user.Password);

                var jsonPayload = JsonConvert.SerializeObject(new[]
                {
            new
            {
                Email = user.Email,
                UserName = user.UserName,
                PasswordEncrpted = encryptedPassword
            }
        });

                using var connection = _context.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@JsonData", jsonPayload);

                await connection.ExecuteAsync("SaveRegisterEmployee", parameters, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("User registered successfully: {UserName}", user.UserName);
                return Ok("Registered Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering user: {UserName}", user.UserName);
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginEmployeeDto loginEmployee)
        {
            try
            {
                _logger.LogInformation("Login attempt for user: {Username}", loginEmployee.Username);

                using var connection = _context.CreateConnection();

                var parameters = new { Username = loginEmployee.Username };

                var user = await connection.QueryFirstOrDefaultAsync<RegisterEmployee>(
                    "LoginEmployee",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                if (user == null)
                {
                    _logger.LogWarning("User not found: {Username}", loginEmployee.Username);
                    return Unauthorized("Invalid username or password");
                }

                var decryptedPassword = _encryptionHelper.Decrypt(user.Password);

                if (decryptedPassword != loginEmployee.Password)
                {
                    _logger.LogWarning("Incorrect password for user: {Username}", loginEmployee.Username);
                    return Unauthorized("Invalid username or password");
                }

                var token = GenerateToken(user.UserName);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login for user: {Username}", loginEmployee.Username);
                return StatusCode(500, $"Internal server error: {ex.Message}");
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
