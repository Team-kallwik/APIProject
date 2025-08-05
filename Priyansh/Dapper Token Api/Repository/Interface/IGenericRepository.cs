using System.Data;

namespace Dapper_Token_Api.Repository.Interface
{
    public interface IGenericRepository<T> where T : class, IBaseEntity
    {
        // Basic CRUD Operations
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> CreateAsync(T entity);
        Task<bool> UpdateAsync(int id, T entity);
        Task<bool> DeleteAsync(int id);
        Task<T?> GetByIdUsingSpAsync(int id, string storedProcedureName);
        Task<IEnumerable<T>> GetAllUsingSpAsync(string storedProcedureName);
        Task<T> CreateUsingSpAsync(object parameters, string storedProcedureName);
        Task<bool> UpdateUsingSpAsync(object parameters, string storedProcedureName);
        Task<bool> DeleteUsingSpAsync(int id, string storedProcedureName);
    }
}
