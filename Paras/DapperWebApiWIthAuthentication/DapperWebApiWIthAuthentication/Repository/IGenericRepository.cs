using System.Threading.Tasks;
using System.Collections.Generic;

namespace DapperWebApiWIthAuthentication.Repository
{
    public interface IGenericRepository<T>
    {
        Task<string> GetAllAsync(string storedProcedure);
        Task<T> GetByIdAsync(string storedProcedure, int id);
        Task CreateAsync(string storedProcedure, object dto);
        Task<bool> UpdateAsync(string storedProcedure, T entity);
        Task<bool> DeleteAsync(string storedProcedure, int id);
    }
}
