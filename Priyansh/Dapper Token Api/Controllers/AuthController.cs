using Dapper_Token_Api.Dto;
using Dapper_Token_Api.Service;
using Microsoft.AspNetCore.Mvc;

namespace Dapper_Token_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var result = await _authService.LoginAsync(loginDto);
                _logger.LogInformation("User {Username} logged in successfully", loginDto.Username);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Failed login attempt for user {Username}: {Message}", loginDto.Username, ex.Message);
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user {Username}", loginDto.Username);
                return BadRequest(new { message = "Login failed" });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(registerDto.Username) ||
                    string.IsNullOrWhiteSpace(registerDto.Email) ||
                    string.IsNullOrWhiteSpace(registerDto.Password))
                {
                    return BadRequest(new { message = "Username, email, and password are required" });
                }

                if (registerDto.Password.Length < 6)
                {
                    return BadRequest(new { message = "Password must be at least 6 characters long" });
                }

                var message = await _authService.RegisterAsync(registerDto);
                _logger.LogInformation("User {Username} registered successfully", registerDto.Username);
                return Ok(new { message = message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Registration failed for user {Username}: {Message}", registerDto.Username, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for user {Username}", registerDto.Username);
                return BadRequest(new { message = "Registration failed" });
            }
        }

    }
}