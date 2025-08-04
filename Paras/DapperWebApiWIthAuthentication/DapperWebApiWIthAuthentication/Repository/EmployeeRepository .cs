using Dapper;
using DapperWebApiWIthAuthentication.Models;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text.Json;

namespace DapperWebApiWIthAuthentication.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DapperContext _context;
        private readonly ILogger<EmployeeRepository> _logger;

        public EmployeeRepository(DapperContext context, ILogger<EmployeeRepository> logger)
        {
             _context = context;
            _logger = logger;
        }

        public async Task<string> GetAllAsync()
        {
            try
            {
                using var connection = _context.CreateConnection();
                _logger.LogInformation("Fetching all employees from database as JSON...");

              
                var jsonResult = await connection.QueryFirstOrDefaultAsync<string>(
                    "GetAllEmployee",
                    commandType: CommandType.StoredProcedure);

                return jsonResult ?? "[]";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllAsync");
                return "[]"; 
            }
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            try
            {
                using var connection = _context.CreateConnection();

                _logger.LogInformation("Fetching employee with ID {Id}", id);

                var json = JsonSerializer.Serialize(new { Id = id });

                _logger.LogDebug("Serialized JSON for GetByIdAsync: {Json}", json);

                return await connection.QueryFirstOrDefaultAsync<Employee>(
                    "GetEmployeeById",
                    new { JsonData = json },
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetByIdAsync for ID {Id}", id);
                return null;
            }
        }

        public async Task CreateAsync(CreateEmployeeDto dto)
        {
            try
            {
                using var connection = _context.CreateConnection();

                var json = JsonSerializer.Serialize(dto);

                await connection.ExecuteAsync(
                    "CreateEmployee",
                    new { JsonData = json },
                    commandType: CommandType.StoredProcedure
                );

                _logger.LogInformation("Employee created using JSON input: {Json}", json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating employee using JSON.");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Employee employee)
        {
            try
            {
                _logger.LogInformation("Starting employee update for ID: {Id}", employee.Id);

                using var connection = _context.CreateConnection();

                var json = JsonSerializer.Serialize(employee);
                _logger.LogDebug("Serialized employee JSON: {Json}", json);

                var result = await connection.ExecuteScalarAsync<int>(
                    "UpdateEmployee",
                    new { JsonData = json },
                    commandType: CommandType.StoredProcedure
                );

                if (result == 1)
                {
                    _logger.LogInformation("Employee with ID {Id} updated successfully.", employee.Id);
                }
                else
                {
                    _logger.LogWarning("Employee with ID {Id} not found or not updated.", employee.Id);
                }

                return result == 1;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating employee with ID: {Id}", employee.Id);
                throw;
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                using var connection = _context.CreateConnection();

                _logger.LogInformation("Attempting to delete employee with ID {Id}", id);

                var json = JsonSerializer.Serialize(new { Id = id });
                _logger.LogDebug("Serialized JSON for DeleteAsync: {Json}", json);

                var result = await connection.QuerySingleOrDefaultAsync<int>(
                    "DeleteEmployee",
                    new { JsonData = json },
                    commandType: CommandType.StoredProcedure
                );

                if (result == 1)
                {
                    _logger.LogInformation("Employee with ID {Id} deleted successfully.", id);
                    return true;
                }
                else
                {
                    _logger.LogWarning("Employee with ID {Id} not found. Delete failed.", id);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting employee with ID {Id}", id);
                throw;
            }
        }

    }
}
