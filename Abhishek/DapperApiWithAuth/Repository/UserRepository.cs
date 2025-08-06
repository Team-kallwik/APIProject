using Dapper;
using DapperApiWithAuth.Interfaces;
using DapperApiWithAuth.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text.Json;
using System.Threading.Tasks;

namespace DapperApiWithAuth.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(IConfiguration config, ILogger<UserRepository> logger)
            : base(config, logger, "User")
        {
        }
        public async Task RegisterAsync(User user)
        {
            try
            {
                using var conn = new MySqlConnection(_config.GetConnectionString("MySqlConnection"));
                await conn.OpenAsync();

                var json = JsonSerializer.Serialize(user, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var parameters = new DynamicParameters();
                parameters.Add("userJson", json, DbType.String);  // Use exact param name from SP

                await conn.ExecuteAsync("RegisterUserJson", parameters, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("User registered.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user");
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
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error fetching user by username");
                throw;
            }
        }
    }
}
