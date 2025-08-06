using Dapper;
using DapperApiWithAuth.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using System.Threading.Tasks;

namespace DapperApiWithAuth.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IConfiguration _config;
        protected readonly ILogger _logger;
        protected readonly string _connectionString;
        protected readonly string _tableName;

        public GenericRepository(IConfiguration config, ILogger logger, string tableName)
        {
            _config = config;
            _logger = logger;
            _connectionString = _config.GetConnectionString("MySqlConnection");
            _tableName = tableName; // Example: "Employee"
        }

        public virtual async Task AddAsync(T entity)
        {
            try
            {
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();

                var json = JsonSerializer.Serialize(entity, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var parameters = new DynamicParameters();
                parameters.Add($"{_tableName.ToLower()}Json", json, DbType.String);

                var spName = $"Add{_tableName}sJson"; // Example: AddEmployeesJson

                await conn.ExecuteAsync(spName, parameters, commandType: CommandType.StoredProcedure);
                _logger.LogInformation($"{_tableName} added.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding {_tableName}");
                throw;
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();

                var spName = $"GetAll{_tableName}s";
                return await conn.QueryAsync<T>(spName, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching all {_tableName}s");
                throw;
            }
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            try
            {
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("empId", id); // Adjust based on your SP parameter

                var spName = $"Get{_tableName}ById";
                return await conn.QuerySingleOrDefaultAsync<T>(spName, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching {_tableName} by id");
                throw;
            }
        }

        public virtual async Task UpdateAsync(T entity)
        {
            try
            {
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();

                var json = JsonSerializer.Serialize(entity, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var parameters = new DynamicParameters();
                parameters.Add("empJson", json, DbType.String);

                var spName = $"Update{_tableName}Json";
                await conn.ExecuteAsync(spName, parameters, commandType: CommandType.StoredProcedure);
                _logger.LogInformation($"{_tableName} updated.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating {_tableName}");
                throw;
            }
        }

        public virtual async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("empId", id);

                var spName = $"Delete{_tableName}ById";
                await conn.ExecuteAsync(spName, parameters, commandType: CommandType.StoredProcedure);
                _logger.LogInformation($"{_tableName} deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting {_tableName}");
                throw;
            }
        }
    }
}

