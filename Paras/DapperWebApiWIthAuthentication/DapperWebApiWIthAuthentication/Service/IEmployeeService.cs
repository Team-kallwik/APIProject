using DapperWebApiWIthAuthentication.Models;

namespace DapperWebApiWIthAuthentication.Service
{
    using DapperWebApiWIthAuthentication.Models;

    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllEmployeeAsync();
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task CreateEmployeeAsync(CreateEmployeeDto dto);
        Task<bool> UpdateEmployeeAsync(Employee employee);
        Task<bool> DeleteEmployeeAsync(int id);
    }

}
