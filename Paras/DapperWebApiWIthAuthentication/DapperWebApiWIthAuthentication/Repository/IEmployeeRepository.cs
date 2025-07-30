using DapperWebApiWIthAuthentication.Models;

namespace DapperWebApiWIthAuthentication.Repository
{
    public interface IEmployeeRepository
    {
        Task<string> GetAllAsync();
        Task<Employee> GetByIdAsync(int id);
        Task CreateAsync(CreateEmployeeDto dto);
        Task <bool> UpdateAsync(Employee employee);
        Task<bool>DeleteAsync(int id);
    }
}
