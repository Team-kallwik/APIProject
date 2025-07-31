using Dapper;
using Dapper_Token_Api.Repository.Interface;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Dapper_Token_Api.Repository.Implementation
{
    public class Repository : IRepository
    {
        private readonly string _connectionString;

        public Repository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? parameters = null, CommandType commandType = CommandType.Text)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QuerySingleOrDefaultAsync<T>(sql, parameters, commandType: commandType);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null, CommandType commandType = CommandType.Text)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<T>(sql, parameters, commandType: commandType);
        }
        public async Task<T> QuerySingleAsync<T>(string sql, object? parameters = null, CommandType commandType = CommandType.Text)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QuerySingleAsync<T>(sql, parameters, commandType: commandType);
        }

        public async Task<int> ExecuteAsync(string sql, object? parameters = null, CommandType commandType = CommandType.Text)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteAsync(sql, parameters, commandType: commandType);
        }

        public async Task<T> ExecuteScalarAsync<T>(string sql, object? parameters = null, CommandType commandType = CommandType.Text)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteScalarAsync<T>(sql, parameters, commandType: commandType);
        }
    }
}