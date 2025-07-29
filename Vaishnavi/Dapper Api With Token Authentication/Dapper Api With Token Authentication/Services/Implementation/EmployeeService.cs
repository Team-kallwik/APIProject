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
            _logger.LogInformation("Service: Getting all employees");
            return await _repo.GetAllAsync();
        }

        public async Task<Emp> GetByIdAsync(int id)
        {
            _logger.LogInformation("Service: Getting employee by ID {Id}", id);
            return await _repo.GetByIdAsync(id);
        }

        public async Task<int> AddAsync(Emp emp)
        {
            _logger.LogInformation("Service: Adding new employee {@Employee}", emp);
            return await _repo.AddAsync(emp);
        }

        public async Task<int> UpdateAsync(Emp emp)
        {
            _logger.LogInformation("Service: Updating employee {@Employee}", emp);
            return await _repo.UpdateAsync(emp);
        }

        public async Task<int> DeleteAsync(int id)
        {
            _logger.LogInformation("Service: Deleting employee with ID {Id}", id);
            return await _repo.DeleteAsync(id);
        }
    }
}
