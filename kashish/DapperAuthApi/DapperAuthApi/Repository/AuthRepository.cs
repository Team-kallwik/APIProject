using Dapper;
using DapperAuthApi.Helpers;
using DapperAuthApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using MySql.Data.MySqlClient;
using DapperAuthApi.Services;

namespace DapperAuthApi.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<AuthRepository> _logger;
        private readonly IJwtAuthManager _jwt;

        public AuthRepository(IConfiguration config, ILogger<AuthRepository> logger, IJwtAuthManager jwt)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
            _logger = logger;
            _jwt = jwt;
        }

        private IDbConnection Connection => new MySqlConnection(_connectionString);

        public async Task<string> RegisterAsync(UserModel user)
        {
            try
            {
                using var connection = Connection;
                var parameters = new DynamicParameters();

                string encryptedPassword = EncryptionHelper.Encrypt(user.Password);
                parameters.Add("p_Username", user.Username);
                parameters.Add("p_Password", encryptedPassword);

                await connection.ExecuteAsync("sp_RegisterUser", parameters, commandType: CommandType.StoredProcedure);
                return "User registered successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration.");
                throw new CustomException("Registration failed.", ex);
            }
        }

        public async Task<string> LoginAsync(UserModel user)
        {
            try
            {
                using var connection = Connection;
                var parameters = new DynamicParameters();
                parameters.Add("p_Username", user.Username);

                var dbUser = await connection.QueryFirstOrDefaultAsync<UserModel>(
                    "sp_GetRegisterByUsername",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                if (dbUser == null)
                    throw new CustomException("User not found.");

                string decryptedPassword = EncryptionHelper.Decrypt(dbUser.Password);
                if (user.Password != decryptedPassword)
                    throw new CustomException("Invalid credentials.");

                return _jwt.GenerateToken(user.Username);
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login.");
                throw new CustomException("Login failed.", ex);
            }
        }
    }
}