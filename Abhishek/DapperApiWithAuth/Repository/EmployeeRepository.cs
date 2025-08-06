using DapperApiWithAuth.DTOs;
using DapperApiWithAuth.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;

namespace DapperApiWithAuth.Repository
{
    public class EmployeeRepository : GenericRepository<EmployeeDto>, IEmployeeRepository
    {
        public EmployeeRepository(IConfiguration config, ILogger<EmployeeRepository> logger)
            : base(config, logger, "Employee")
        {
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

                await conn.ExecuteAsync("AddEmployeesJson", parameters, commandType: CommandType.StoredProcedure);
                _logger.LogInformation("Employees added in bulk.");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error adding employees in bulk.");
                throw;
            }
        }
    }
}
