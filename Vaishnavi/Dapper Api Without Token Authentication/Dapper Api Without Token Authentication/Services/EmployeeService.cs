using Dapper_Api_Without_Token_Authentication.Models;
using Dapper_Api_Without_Token_Authentication.Repository;

namespace Dapper_Api_Without_Token_Authentication.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<Employee> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task<int> AddAsync(Employee employee) => await _repository.AddAsync(employee);
        public async Task<int> UpdateAsync(Employee employee) => await _repository.UpdateAsync(employee);
        public async Task<int> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
