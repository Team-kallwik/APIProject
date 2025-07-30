using Dapper_Api_With_Token_Authentication.Model;
using Dapper_Api_With_Token_Authentication.Repository.Interface;
using Dapper_Api_With_Token_Authentication.Services.Interface;
using System.Text.Json;

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
            var jsonResult = await _repo.GetAllAsJsonAsync("GetAllEmployees");
            return JsonSerializer.Deserialize<IEnumerable<Emp>>(jsonResult);
        }


        public async Task<Emp> GetByIdAsync(int id)
        {
            var jsonParam = JsonSerializer.Serialize(new { Id = id });
            return await _repo.GetByIdAsync("GetEmployeeById", new { JsonData = jsonParam });
        }


        public async Task<int> AddAsync(Emp emp)
        {
            var json = JsonSerializer.Serialize(new[] { emp });
            return await _repo.AddAsync("AddEmployeeFromJson", new { json });
        }

        public async Task<int> UpdateAsync(Emp emp)
        {
            var json = JsonSerializer.Serialize(new[] { emp });
            return await _repo.UpdateAsync("UpdateEmployeeFromJson", new { json });
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync("DeleteEmployeeById", new { Id = id });
        }
    }
}
