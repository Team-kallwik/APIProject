using DapperWebApiWIthAuthentication.Models;

namespace DapperWebApiWIthAuthentication.Repository
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee> GetByIdAsync(int id);
        Task CreateAsync(CreateEmployeeDto dto);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(int id);
    }
}
