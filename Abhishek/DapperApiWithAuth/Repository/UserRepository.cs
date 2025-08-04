using DapperApiWithAuth.Interfaces;
using DapperApiWithAuth.Models;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Text.Json;

namespace DapperApiWithAuth.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _config;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(IConfiguration config, ILogger<UserRepository> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task RegisterAsync(User user)
        {
            try
            {
                using var conn = new MySqlConnection(_config.GetConnectionString("MySqlConnection"));
                await conn.OpenAsync();

                var userJson = JsonSerializer.Serialize(new
                {
                    username = user.Username,
                    passwordHash = user.PasswordHash,
                    email = user.Email
                });
                var parameters = new DynamicParameters();
                parameters.Add("UserJson", userJson);

                await conn.ExecuteAsync("RegisterUserJson", parameters, commandType: System.Data.CommandType.StoredProcedure);
                _logger.LogInformation("User registered: {Username}", user.Username);

            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error During User Registered");
                throw;
            
            }
        }
        public async Task<User> GetByUsernameAsync(string username)
        {
            try
            {
                using var conn = new MySqlConnection(_config.GetConnectionString("MySqlConnection"));
                await conn.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("uname", username);

                return await conn.QuerySingleOrDefaultAsync<User>("GetUserByUsername", parameters, commandType: System.Data.CommandType.StoredProcedure);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error fatching by username");
                throw;
            }
        }
    }
}
