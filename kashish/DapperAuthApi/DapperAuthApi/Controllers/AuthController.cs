using DapperAuthApi.Models;
using DapperAuthApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IJwtAuthManager _jwt;

    public AuthController(IJwtAuthManager jwt)
    {
        _jwt = jwt;
    }

    [HttpPost("login")]
    public IActionResult Login(UserModel model)
    {
        try
        {
            // Dummy authentication
            if (model.Username == "admin" && model.Password == "123")
            {
                var token = _jwt.GenerateToken(model.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized(new { Message = "Invalid username or password." });
        }
        catch (Exception ex)
        {
            // Log the exception if needed (e.g., using ILogger)
            return StatusCode(500, new { Message = "An error occurred during login.", Error = ex.Message });
        }
    }
}