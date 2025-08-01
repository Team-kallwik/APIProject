using Dapper;
using DapperAuthApi.Controllers;
using DapperAuthApi.Helpers;
using DapperAuthApi.Models;
using DapperAuthApi.Repository;
using DapperAuthApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IJwtAuthManager _jwt;
    private readonly IEmploRepository _repo;
    private readonly ILogger<EmploController> _logger;
    private readonly IConfiguration _configuration;

    public AuthController(IJwtAuthManager jwt, IEmploRepository repo)
    {
        _jwt = jwt;
        _repo = repo;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserModel model)
    {
        var result = await _repo.LoginAsync(model); 
        return Ok(new { message = result });
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserModel user)
    {
        var result = await _repo.RegisterAsync(user); // Fix: _repo now has RegisterAsync method
        return Ok(new { message = result });
    }
}