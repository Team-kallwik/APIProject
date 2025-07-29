using DapperWebApiWIthAuthentication.Models;

namespace DapperWebApiWIthAuthentication.Service
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllEmployeeAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task CreateEmployeeAsync(CreateEmployeeDto dto);
        Task UpdateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(int id);
    }
}
