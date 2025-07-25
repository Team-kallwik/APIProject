using DapperConfig.Models;


namespace DapperConfig.Repository
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Emp>> GetAllAsync();
        Task<Emp> GetByIdAsync(int id);
        Task<int> AddAsync(Emp employee);
        Task<int> UpdateAsync(Emp employee);
        Task<int> DeleteAsync(int id);
    }
}
