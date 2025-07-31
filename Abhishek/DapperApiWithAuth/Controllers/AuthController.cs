using DapperApiWithAuth.DTOs;
using DapperApiWithAuth.Interfaces;
using DapperApiWithAuth.Models;
using DapperApiWithAuth.Service;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Generators;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using BCrypt.Net;


namespace DapperApiWithAuth.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserRepository userRepo, ITokenService tokenservice, ILogger<AuthController> logger) 
        {
            _userRepo = userRepo;
            _tokenService = tokenservice;   
            _logger = logger;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            try
            {
                var exitinguser = await _userRepo.GetByUsernameAsync(dto.Username);
                if (exitinguser != null)
                
                    return BadRequest("username already token");
                
                    var user = new User
                    {

                        Username = dto.Username,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                        Email = dto.Email
                    };
                await _userRepo.RegisterAsync(user);
                return Ok("user registerd successfully");
            }    
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Registration Failed");
                return StatusCode(500, "Server Error");

            }

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try
            {
                var user = await _userRepo.GetByUsernameAsync(dto.Username);

                // Correct Password Verification
                if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                    return Unauthorized("Invalid Username or Password");

                var token = _tokenService.CreateToken(user);

                return Ok(new { token }); // Return token in response
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login Failed");
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

    }
}
