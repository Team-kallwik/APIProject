using Dapper_Api_Without_Token_Authentication.Models;

namespace Dapper_Api_Without_Token_Authentication.Repository
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee> GetByIdAsync(int id);
        Task<int> AddAsync(Employee employee);
        Task<int> UpdateAsync(Employee employee);
        Task<int> DeleteAsync(int id);
    }
}
