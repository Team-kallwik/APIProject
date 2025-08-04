using DapperAPI.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DapperAPI.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<int> AddAsync(T entity);
        Task EnsureTableExistsAsync();
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(int id);
    }
}
