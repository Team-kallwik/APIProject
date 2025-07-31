using Dapper;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DapperAuthApi.Repository
{
public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly string _connectionString;

        public GenericRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected IDbConnection Connection => new MySqlConnection(_connectionString);

        public async Task<IEnumerable<T>> GetAllAsync(string storedProcedure)
        {
            using var conn = Connection;
            return await conn.QueryAsync<T>(storedProcedure, commandType: CommandType.StoredProcedure);
        }

        public async Task<T?> GetByIdAsync(string storedProcedure, object parameters)
        {
            using var conn = Connection;
            return await conn.QueryFirstOrDefaultAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> CreateAsync(string storedProcedure, object parameters)
        {
            using var conn = Connection;
            return await conn.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpdateAsync(string storedProcedure, object parameters)
        {
            using var conn = Connection;
            return await conn.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> DeleteAsync(string storedProcedure, object parameters)
        {
            using var conn = Connection;
            return await conn.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}