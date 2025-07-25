using System.Linq.Expressions;

namespace DapperAPI.Repositories
{
    public interface ICustomer<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(int id);
    }
}