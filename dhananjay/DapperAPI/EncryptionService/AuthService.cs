using Dapper;
using DapperAPI.Data;
using DapperAPI.DTOs;
using DapperAPI.Model;
using DapperAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using DapperAPI.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DapperAPI.Exceptions;

namespace DapperAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly DapperContext _context;
        private readonly IConfiguration _config;

        public AuthService(DapperContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<string> RegisterAsync(UserDto request)
        {
            try
            {
                using var conn = _context.CreateConnection();

                var existingUser = await conn.QueryFirstOrDefaultAsync<User>(
                    "GetUserByEmail",
                    new { Email = request.Email },
                    commandType: CommandType.StoredProcedure);

                if (existingUser != null)
                    throw new ResourceConflictException();

                CreatePasswordHash(request.Password, out byte[] hash, out byte[] salt);

                await conn.ExecuteAsync(
                    "AddUser",
                    new
                    {
                        Email = request.Email,
                        PasswordHash = hash,
                        PasswordSalt = salt
                    },
                    commandType: CommandType.StoredProcedure);

                return CreateToken(request.Email);
            }
            catch (Exception ex)
            {
                
                return $"[Error]: {ex.Message}";
                
            }
        }

        public async Task<string> LoginAsync(UserDto request)
        {
            try
            {
                using var conn = _context.CreateConnection();


                var user = await conn.QueryFirstOrDefaultAsync<User>(
                    "GetUserByEmail",
                    new { Email = request.Email },
                    commandType: CommandType.StoredProcedure);

                if (user == null || !VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
                    return "Invalid credentials.";

                return CreateToken(user.Email);
            }
            catch (Exception ex)
            {
                return $"[Error]: {ex.Message}";
            }
        }

        private string CreateToken(string email)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA512();
            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(storedHash);
        }
    }
}


