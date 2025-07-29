using Dapper_Api_With_Token_Authentication.Model;
using Dapper_Api_With_Token_Authentication.Repository.Interface;
using Dapper_Api_With_Token_Authentication.Services.Interface;

namespace Dapper_Api_With_Token_Authentication.Services.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repo;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(IEmployeeRepository repo, ILogger<EmployeeService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<IEnumerable<Emp>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Service: Getting all employees");
                return await _repo.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service: Error getting all employees");
                throw;
            }
        }

        public async Task<Emp> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Service: Getting employee by ID {Id}", id);
                return await _repo.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service: Error getting employee by ID {Id}", id);
                throw;
            }
        }

        public async Task<int> AddAsync(Emp emp)
        {
            try
            {
                _logger.LogInformation("Service: Adding new employee {@Employee}", emp);
                return await _repo.AddAsync(emp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service: Error adding employee {@Employee}", emp);
                throw;
            }
        }

        public async Task<int> UpdateAsync(Emp emp)
        {
            try
            {
                _logger.LogInformation("Service: Updating employee {@Employee}", emp);
                return await _repo.UpdateAsync(emp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service: Error updating employee {@Employee}", emp);
                throw;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            try
            {
                _logger.LogInformation("Service: Deleting employee with ID {Id}", id);
                return await _repo.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service: Error deleting employee ID {Id}", id);
                throw;
            }
        }
    }
}
