using DapperConfiguration.Models;

namespace DapperConfiguration.Services
{
    public interface IEmploService
    {
        Task<IEnumerable<Emplo>> GetAllAsync();
        Task<Emplo> GetByIdAsync(int id);
        Task<int> CreateAsync(Emplo employee);
        Task<int> UpdateAsync(Emplo employee);
        Task<int> DeleteAsync(int id);
    }
}
