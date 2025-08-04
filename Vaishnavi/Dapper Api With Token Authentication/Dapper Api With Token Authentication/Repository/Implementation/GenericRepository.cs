using Dapper;
using Dapper_Api_With_Token_Authentication.Repository.Interface;
using Dapper_Api_With_Token_Authentication.Exceptions; 
using Microsoft.Data.SqlClient;
using System.Data;

namespace Dapper_Api_With_Token_Authentication.Repository.Imp
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IConfiguration _config;
        private readonly ILogger<GenericRepository<T>> _logger;
        private readonly string _connectionString;

        public GenericRepository(IConfiguration config, ILogger<GenericRepository<T>> logger)
        {
            _config = config;
            _logger = logger;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);

        public async Task<string> GetAllAsJsonAsync(string spName)
        {
            try
            {
                _logger.LogInformation("GenericRepo: Executing SP {SpName} (JSON)", spName);
                using var db = Connection;
                return await db.ExecuteScalarAsync<string>(spName, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "GenericRepo: SQL Error executing SP {SpName}", spName);
                throw new DatabaseException($"Database error while executing {spName}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GenericRepo: Unexpected error in SP {SpName}", spName);
                throw;
            }
        }

        public async Task<T> GetByIdAsync(string spName, object jsonParameters)
        {
            try
            {
                _logger.LogInformation("GenericRepo: Executing SP {SpName} with JSON params {@Params}", spName, jsonParameters);
                using var db = Connection;

                return await db.QueryFirstOrDefaultAsync<T>(
                    spName,
                    jsonParameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "GenericRepo: SQL Error executing SP {SpName}", spName);
                throw new DatabaseException($"Database error while executing {spName}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GenericRepo: Unexpected error in SP {SpName}", spName);
                throw;
            }
        }

        public async Task<int> AddAsync(string spName, object parameters)
        {
            try
            {
                _logger.LogInformation("GenericRepo: Adding entity using SP {SpName}", spName);
                using var db = Connection;

                var dynamicParams = new DynamicParameters(parameters);
                dynamicParams.Add("Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await db.ExecuteAsync(spName, dynamicParams, commandType: CommandType.StoredProcedure);
                return dynamicParams.Get<int>("Result");
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "GenericRepo: SQL Error adding entity using SP {SpName}", spName);
                throw new DatabaseException($"Database error while adding entity using {spName}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GenericRepo: Unexpected error adding entity using SP {SpName}", spName);
                throw;
            }
        }

        public async Task<int> UpdateAsync(string spName, object parameters)
        {
            try
            {
                _logger.LogInformation("GenericRepo: Updating entity using SP {SpName}", spName);
                using var db = Connection;

                var dynamicParams = new DynamicParameters(parameters);
                dynamicParams.Add("Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await db.ExecuteAsync(spName, dynamicParams, commandType: CommandType.StoredProcedure);
                return dynamicParams.Get<int>("Result");
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "GenericRepo: SQL Error updating entity using SP {SpName}", spName);
                throw new DatabaseException($"Database error while updating entity using {spName}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GenericRepo: Unexpected error updating entity using SP {SpName}", spName);
                throw;
            }
        }

        public async Task<int> DeleteAsync(string spName, object parameters)
        {
            try
            {
                _logger.LogInformation("GenericRepo: Deleting entity using SP {SpName}", spName);
                using var db = Connection;
                return await db.ExecuteAsync(spName, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "GenericRepo: SQL Error deleting entity using SP {SpName}", spName);
                throw new DatabaseException($"Database error while deleting entity using {spName}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GenericRepo: Unexpected error deleting entity using SP {SpName}", spName);
                throw;
            }
        }
    }
}
