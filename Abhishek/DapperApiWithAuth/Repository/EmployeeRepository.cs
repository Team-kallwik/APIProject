using Dapper;
using DapperApiWithAuth.DTOs;
using DapperApiWithAuth.Interfaces;
using MySql.Data.MySqlClient;
using System.Data;
using System.Linq.Expressions;
using System.Text.Json;

namespace DapperApiWithAuth.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public readonly IConfiguration _config;
        public readonly ILogger<EmployeeRepository> _logger;

        public EmployeeRepository(IConfiguration config, ILogger<EmployeeRepository> logger) 
        {
            _config = config;
            _logger = logger;
        }


        public async Task AddBulkAsync(IEnumerable<EmployeeDto> employees)
        {
            try
            {
                using var conn = new MySqlConnection(_config.GetConnectionString("MySqlConnection"));
                await conn.OpenAsync();

                var json = JsonSerializer.Serialize(employees, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var parameters = new DynamicParameters();
                parameters.Add("empJson", json, DbType.String);

                await conn.ExecuteAsync("AddEmployeesJson", parameters, commandType: System.Data.CommandType.StoredProcedure);
                _logger.LogInformation("Employee Added In Bulk");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Adding Employees in Bulk");
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = new MySqlConnection(_config.GetConnectionString("MySqlConnection"));
                await conn.OpenAsync();

                var parameters = new DynamicParameters();
               parameters.Add("empId", id);

                await conn.ExecuteAsync("DeleteEmployeeById", parameters, commandType: System.Data.CommandType.StoredProcedure);
                _logger.LogInformation("Employee deleted: {Id}", id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Employee");
                throw;
            }
            
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            try
            {
                using var conn = new MySqlConnection(_config.GetConnectionString("MySqlConnection"));
                await conn.OpenAsync();

                return await conn.QueryAsync<EmployeeDto>("GetAllEmployees", commandType: System.Data.CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error fetching employee");
                throw;
            }
        }

        public async Task<EmployeeDto> GetByIdAsync(int id)
        {
            try
            {
                using var conn = new MySqlConnection(_config.GetConnectionString("MySqlConnection"));
                await conn.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("empId", id);

                return await conn.QuerySingleOrDefaultAsync<EmployeeDto>("GetEmployeeById", parameters, commandType: System.Data.CommandType.StoredProcedure);

            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "error fetching employee by id");
                throw;
            }
        }

        public async Task UpdateAsync(EmployeeDto emp)
        {
            try
            {
                using var conn = new MySqlConnection(_config.GetConnectionString("MySqlConnection"));
                await conn.OpenAsync();

                var json = JsonSerializer.Serialize(emp, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var parameters = new DynamicParameters();
                parameters.Add("empjson", json);

                await conn.ExecuteAsync("UpdateEmployeeJson", parameters, commandType: System.Data.CommandType.StoredProcedure);
                _logger.LogInformation("Update Employee By: {Id}", emp.Id); 

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error Updating Employee");
                throw;
            }
        }
    }
}
