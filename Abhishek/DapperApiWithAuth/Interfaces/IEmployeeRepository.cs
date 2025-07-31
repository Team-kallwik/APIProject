using DapperApiWithAuth.DTOs;

namespace DapperApiWithAuth.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<EmployeeDto>> GetAllAsync();
        Task<EmployeeDto> GetByIdAsync(int id);
        Task AddBulkAsync(IEnumerable<EmployeeDto> employees);
        Task UpdateAsync(EmployeeDto emp);
        Task DeleteAsync(int id);
    }
}
