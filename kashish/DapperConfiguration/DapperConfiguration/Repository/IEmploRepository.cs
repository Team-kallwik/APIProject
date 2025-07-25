using DapperConfiguration.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DapperConfiguration.Repository
{
    public interface IEmploRepository
    {
        Task<IEnumerable<Emplo>> GetAllAsync();
        Task<Emplo> GetByIdAsync(int id);
        Task<int> CreateAsync(Emplo employee);
        Task<int> UpdateAsync(Emplo employee);
        Task<int> DeleteAsync(int id);
    }
}
