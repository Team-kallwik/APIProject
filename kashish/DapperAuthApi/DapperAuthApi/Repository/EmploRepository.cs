using Dapper;
using DapperAuthApi.Helpers;
using DapperAuthApi.Models;
using DapperAuthApi.Services;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System.Data;

namespace DapperAuthApi.Repository
{
    public class EmploRepository : GenericRepository<Emplo>, IEmploRepository
    {
        private readonly ILogger<EmploRepository> _logger;
        private readonly IJwtAuthManager _jwt;

        public EmploRepository(IConfiguration config, ILogger<EmploRepository> logger, IJwtAuthManager jwt)
            : base(config.GetConnectionString("DefaultConnection"))
        {
            _jwt = jwt;
            _logger = logger;
        }

        public async Task<IEnumerable<Emplo>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Getting all employees...");
                return await base.GetAllAsync("sp_GetAllEmp");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetAllAsync.");
                throw;
            }
        }

        public async Task<Emplo> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Getting employee with Id = {id}");
                return await base.GetByIdAsync("sp_GetEmpById", new { p_Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred in GetByIdAsync for Id = {id}");
                throw;
            }
        }

        public async Task<int> CreateAsync(Emplo emp)
        {
            try
            {
                _logger.LogInformation($"Creating employee: {emp.Name}");
                return await base.CreateAsync("sp_InsertEmp", new
                {
                    p_Name = emp.Name,
                    p_Department = emp.Department,
                    p_Salary = emp.Salary
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while creating employee: {emp.Name}");
                throw;
            }
        }

        public async Task<int> UpdateAsync(Emplo emp)
        {
            try
            {
                _logger.LogInformation($"Updating employee with Id = {emp.Id}");
                return await base.UpdateAsync("sp_UpdateEmp", new
                {
                    p_Id = emp.Id,
                    p_Name = emp.Name,
                    p_Department = emp.Department,
                    p_Salary = emp.Salary
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating employee with Id = {emp.Id}");
                throw;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting employee with Id = {id}");
                return await base.DeleteAsync("sp_DeleteEmp", new { p_Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting employee with Id = {id}");
                throw;
            }
        }
        public async Task<string> RegisterAsync(UserModel user)
        {
            try
            {
                using var connection = Connection;
                var parameters = new DynamicParameters();

                // Encrypt password if needed
                string encryptedPassword = EncryptionHelper.Encrypt(user.Password);
                parameters.Add("p_Username", user.Username);
                parameters.Add("p_Password", encryptedPassword);

                await connection.ExecuteAsync("sp_RegisterUser", parameters, commandType: CommandType.StoredProcedure);

                return "User registered successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in RegisterAsync: {ex.Message}");
                throw;
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
                    commandType: CommandType.StoredProcedure
                );

                if (dbUser == null)
                {
                    _logger.LogWarning("Login failed: User not found.");
                    return null;
                }

                string decryptedPassword = EncryptionHelper.Decrypt(dbUser.Password);

                if (user.Password != decryptedPassword)
                {
                    _logger.LogWarning("Login failed: Incorrect password.");
                    return null;
                }

                string token = _jwt.GenerateToken(user.Username);
                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in LoginAsync");
                throw;
            }
        }
    
    }
}
    