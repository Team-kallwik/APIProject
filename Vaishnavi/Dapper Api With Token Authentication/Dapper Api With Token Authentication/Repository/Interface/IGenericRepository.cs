namespace Dapper_Api_With_Token_Authentication.Repository.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<string> GetAllAsJsonAsync(string spName);
        Task<T> GetByIdAsync(string spName, object parameters);
        Task<int> AddAsync(string spName, object parameters);
        Task<int> UpdateAsync(string spName, object parameters);
        Task<int> DeleteAsync(string spName, object parameters);
    }
}
