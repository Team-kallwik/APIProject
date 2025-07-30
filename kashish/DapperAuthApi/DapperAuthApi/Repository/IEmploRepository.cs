using DapperAuthApi.Models;

namespace DapperAuthApi.Repository
{
    public interface IEmploRepository
    {
        Task<IEnumerable<Emplo>> GetAllAsync();
        Task<Emplo> GetByIdAsync(int id);
        Task<int> CreateAsync(Emplo emp);
        Task<int> UpdateAsync(Emplo emp);
        Task<int> DeleteAsync(int id);
    }
}
