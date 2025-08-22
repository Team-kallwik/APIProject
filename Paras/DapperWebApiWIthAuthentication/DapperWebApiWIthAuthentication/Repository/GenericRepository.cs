using Dapper;
using DapperWebApiWIthAuthentication.Models;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text.Json;

namespace DapperWebApiWIthAuthentication.Repository
{
    public class GenericRepository<T> : IGenericRepository<T>
    {
        private readonly DapperContext _context;
        private readonly ILogger<GenericRepository<T>> _logger;

        public GenericRepository(DapperContext context, ILogger<GenericRepository<T>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<string> GetAllAsync(string storedProcedure)
        {
            try
            {
                using var connection = _context.CreateConnection();
                _logger.LogInformation("Fetching all records using {StoredProcedure}", storedProcedure);

                var result = await connection.QueryFirstOrDefaultAsync<string>(
                    storedProcedure,
                    commandType: CommandType.StoredProcedure);

                return result ?? "[]";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllAsync");
                return "[]";
            }
        }

        public async Task<T> GetByIdAsync(string storedProcedure, int id)
        {
            try
            {
                using var connection = _context.CreateConnection();
                var json = JsonSerializer.Serialize(new { Id = id });

                _logger.LogInformation("Fetching record with ID {Id} using {StoredProcedure}", id, storedProcedure);
                return await connection.QueryFirstOrDefaultAsync<T>(
                    storedProcedure,
                    new { JsonData = json },
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetByIdAsync");
                return default!;
            }
        }

        public async Task CreateAsync(string storedProcedure, object dto)
        {
            try
            {
                using var connection = _context.CreateConnection();
                var json = JsonSerializer.Serialize(dto);

                _logger.LogInformation("Creating record using {StoredProcedure}", storedProcedure);
                await connection.ExecuteAsync(
                    storedProcedure,
                    new { JsonData = json },
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateAsync");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(string storedProcedure, T entity)
        {
            try
            {
                using var connection = _context.CreateConnection();
                var json = JsonSerializer.Serialize(entity);

                _logger.LogInformation("Updating record using {StoredProcedure}", storedProcedure);
                var result = await connection.ExecuteScalarAsync<int>(
                    storedProcedure,
                    new { JsonData = json },
                    commandType: CommandType.StoredProcedure);

                return result == 1;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateAsync");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(string storedProcedure, int id)
        {
            try
            {
                using var connection = _context.CreateConnection();
                var json = JsonSerializer.Serialize(new { Id = id });

                _logger.LogInformation("Deleting record with ID {Id} using {StoredProcedure}", id, storedProcedure);
                var result = await connection.ExecuteScalarAsync<int>(
                    storedProcedure,
                    new { JsonData = json },
                    commandType: CommandType.StoredProcedure);

                return result == 1;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteAsync");
                throw;
            }
        }
    }
}
