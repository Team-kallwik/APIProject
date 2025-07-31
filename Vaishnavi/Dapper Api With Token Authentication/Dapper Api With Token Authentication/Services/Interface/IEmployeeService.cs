using Dapper_Api_With_Token_Authentication.Model;

namespace Dapper_Api_With_Token_Authentication.Services.Interface
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Emp>> GetAllAsync();
        Task<Emp> GetByIdAsync(int id);
        Task<int> AddAsync(Emp emp);
        Task<int> UpdateAsync(Emp emp);
        Task<int> DeleteAsync(int id);
    }
}
