using DapperAuthApi.Models;

namespace DapperAuthApi.Repository
{   
    public interface IEmploRepository : IGenericRepository<Emplo>
        {
        Task<IEnumerable<Emplo>> GetAllAsync();
        Task<Emplo> GetByIdAsync(int id);
        Task<int> CreateAsync(Emplo emp);
        Task<int> UpdateAsync(Emplo emp);
        Task<int> DeleteAsync(int id);
        Task<string> RegisterAsync(UserModel user);
        Task<string> LoginAsync(UserModel user);
    }
}
