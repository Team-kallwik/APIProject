using DapperAuthApi.Models;
using Microsoft.Extensions.Logging;

namespace DapperAuthApi.Repository
{
    public class EmploRepository : GenericRepository<Emplo>, IEmploRepository
    {
        private readonly ILogger<EmploRepository> _logger;

        public EmploRepository(IConfiguration config, ILogger<EmploRepository> logger)
            : base(config.GetConnectionString("DefaultConnection"))
        {
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
    }
}