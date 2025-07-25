﻿using DapperAPI.DTOs;
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
        private readonly ILogger<CustomerRepository> _logger;

        public AuthController(AuthService auth, ILogger<CustomerRepository> logger)
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

                if (result == "User already exists.")
                    return Conflict(result);

                if (result == "Database error occurred." || result == "An unexpected error occurred.")
                    return StatusCode(500, result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[Controller Error - Register] {ex.Message}");
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
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[Controller Error - Login] {ex.Message}");
                return StatusCode(500, "Internal server error during login.");
            }
        }
    }
}
