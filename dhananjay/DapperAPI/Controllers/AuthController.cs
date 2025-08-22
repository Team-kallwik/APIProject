using DapperAPI.DTOs;
using DapperAPI.Exceptions;
using DapperAPI.Model;
using DapperAPI.Repositories;
using DapperAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DapperAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _auth;
        private readonly ILogger<IGenericRepository<Customer>> _logger;

        public AuthController(AuthService auth, ILogger<IGenericRepository<Customer>> logger)
        {
            _auth = auth;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto dto)
        {
            try
            {
                var result = await _auth.RegisterAsync(dto);
                return Ok(result);
            }
            catch(ResourceConflictException ex)
            {
                _logger.LogError(ex, "User already exists");
                return NotFound("Data exists");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration");
                return StatusCode(500, "Internal server error during registration.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto dto)
        {
            try
            {
                var result = await _auth.LoginAsync(dto);

                if (string.IsNullOrEmpty(result))
                    return Unauthorized("Invalid credentials.");

                return Ok(result);
            }
            catch(InvalidCredentialsException ex)
            {
                _logger.LogError(ex, "Invalid credentials provided during login.");
                return Unauthorized("Invalid credentials.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[Controller Error - Login] {ex.Message}");
                return StatusCode(500, "Internal server error during login.");
            }

        }

    }
}
