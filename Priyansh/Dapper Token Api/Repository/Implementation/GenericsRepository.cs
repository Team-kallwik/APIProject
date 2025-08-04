using Dapper;
using Dapper_Token_Api.Repository.Interface;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using System.Text;

namespace Dapper_Token_Api.Repository.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IBaseEntity
    {
        private readonly IRepository _repository;
        private readonly string _tableName;

        public GenericRepository(IRepository repository)
        {
            _repository = repository;
            _tableName = GetTableName();
        }

        private string GetTableName()
        {
            var type = typeof(T);
            var tableAttribute = type.GetCustomAttribute<TableAttribute>();
            return tableAttribute?.Name ?? type.Name + "s"; // Default pluralization
        }

        private string GetColumnNames(bool excludeId = false)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => p.CanWrite && (!excludeId || p.Name != "Id"));

            return string.Join(", ", properties.Select(p => p.Name));
        }

        private string GetParameterNames(bool excludeId = false)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => p.CanWrite && (!excludeId || p.Name != "Id"));

            return string.Join(", ", properties.Select(p => "@" + p.Name));
        }

        private string GetUpdateSetClause()
        {
            var properties = typeof(T).GetProperties()
                .Where(p => p.CanWrite && p.Name != "Id");

            return string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));
        }

        // Basic CRUD Operations
        public virtual async Task<T?> GetByIdAsync(int id)
        {
            var sql = $"SELECT * FROM {_tableName} WHERE Id = @Id";
            return await _repository.QuerySingleOrDefaultAsync<T>(sql, new { Id = id });
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var sql = $"SELECT * FROM {_tableName}";
            return await _repository.QueryAsync<T>(sql);
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            var columnNames = GetColumnNames(excludeId: true);
            var parameterNames = GetParameterNames(excludeId: true);

            var sql = $@"
                INSERT INTO {_tableName} ({columnNames}) 
                OUTPUT INSERTED.*
                VALUES ({parameterNames})";

            return await _repository.QuerySingleAsync<T>(sql, entity);
        }

        public virtual async Task<bool> UpdateAsync(int id, T entity)
        {
            var setClause = GetUpdateSetClause();
            var sql = $"UPDATE {_tableName} SET {setClause} WHERE Id = @Id";

            // Create parameters object with Id
            var parameters = new DynamicParameters(entity);
            parameters.Add("Id", id);

            var rowsAffected = await _repository.ExecuteAsync(sql, parameters);
            return rowsAffected > 0;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var sql = $"DELETE FROM {_tableName} WHERE Id = @Id";
            var rowsAffected = await _repository.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        // Additional common operations
        public virtual async Task<IEnumerable<T>> GetByConditionAsync(string whereClause, object? parameters = null)
        {
            var sql = $"SELECT * FROM {_tableName} WHERE {whereClause}";
            return await _repository.QueryAsync<T>(sql, parameters);
        }

        public virtual async Task<T?> GetSingleByConditionAsync(string whereClause, object? parameters = null)
        {
            var sql = $"SELECT * FROM {_tableName} WHERE {whereClause}";
            return await _repository.QuerySingleOrDefaultAsync<T>(sql, parameters);
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            var sql = $"SELECT COUNT(1) FROM {_tableName} WHERE Id = @Id";
            var count = await _repository.ExecuteScalarAsync<int>(sql, new { Id = id });
            return count > 0;
        }

        public virtual async Task<int> CountAsync()
        {
            var sql = $"SELECT COUNT(*) FROM {_tableName}";
            return await _repository.ExecuteScalarAsync<int>(sql);
        }

        public virtual async Task<int> CountByConditionAsync(string whereClause, object? parameters = null)
        {
            var sql = $"SELECT COUNT(*) FROM {_tableName} WHERE {whereClause}";
            return await _repository.ExecuteScalarAsync<int>(sql, parameters);
        }

        // Stored procedure operations
        public virtual async Task<T?> GetByIdUsingSpAsync(int id, string storedProcedureName)
        {
            var parameters = new { Id = id };
            return await _repository.QuerySingleOrDefaultAsync<T>(
                storedProcedureName, parameters, CommandType.StoredProcedure);
        }

        public virtual async Task<IEnumerable<T>> GetAllUsingSpAsync(string storedProcedureName)
        {
            return await _repository.QueryAsync<T>(
                storedProcedureName, commandType: CommandType.StoredProcedure);
        }

        public virtual async Task<T> CreateUsingSpAsync(object parameters, string storedProcedureName)
        {
            return await _repository.QuerySingleAsync<T>(
                storedProcedureName, parameters, CommandType.StoredProcedure);
        }

        public virtual async Task<bool> UpdateUsingSpAsync(object parameters, string storedProcedureName)
        {
            var rowsAffected = await _repository.ExecuteAsync(
                storedProcedureName, parameters, CommandType.StoredProcedure);
            return rowsAffected > 0;
        }

        public virtual async Task<bool> DeleteUsingSpAsync(int id, string storedProcedureName)
        {
            var parameters = new { Id = id };
            var rowsAffected = await _repository.ExecuteAsync(
                storedProcedureName, parameters, CommandType.StoredProcedure);
            return rowsAffected > 0;
        }
    }
}