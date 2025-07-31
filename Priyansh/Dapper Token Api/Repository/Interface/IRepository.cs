using System.Data;

namespace Dapper_Token_Api.Repository.Interface
{
    public interface IRepository
    {
        Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? parameters = null, CommandType commandType = CommandType.Text);
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null, CommandType commandType = CommandType.Text);
        Task<T> QuerySingleAsync<T>(string sql, object? parameters = null, CommandType commandType = CommandType.Text);
        Task<int> ExecuteAsync(string sql, object? parameters = null, CommandType commandType = CommandType.Text);
        Task<T> ExecuteScalarAsync<T>(string sql, object? parameters = null, CommandType commandType = CommandType.Text);
    }
}