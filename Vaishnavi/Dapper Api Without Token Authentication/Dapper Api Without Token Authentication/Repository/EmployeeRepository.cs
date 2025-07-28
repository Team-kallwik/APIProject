using Dapper_Api_Without_Token_Authentication.Models;
using System.Data;
using System.Text.Json;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Dapper_Api_Without_Token_Authentication.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IConfiguration _config;

        public EmployeeRepository(IConfiguration config)
        {
            _config = config;
        }

        private IDbConnection Connection => new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            using var conn = Connection;
            return await conn.QueryAsync<Employee>("GetAllEmployees", commandType: CommandType.StoredProcedure);
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            using var conn = Connection;
            var result = await conn.QueryFirstOrDefaultAsync<Employee>("GetEmployeeById", new { Id = id }, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<int> AddAsync(Employee employee)
        {
            var json = JsonSerializer.Serialize(employee);
            using var conn = Connection;
            var parameters = new DynamicParameters();
            parameters.Add("@json", json, DbType.String);

            var result = await conn.QuerySingleAsync<int>(
                "CreateEmployeeFromJson",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            return result;
        }
        public async Task<int> UpdateAsync(Employee employee)
        {
            var json = JsonSerializer.Serialize(employee);
            using var conn = Connection;
            var parameters = new DynamicParameters();
            parameters.Add("@json", json, DbType.String);

            var result = await conn.QuerySingleAsync<int>(
                "UpdateEmployeeFromJson",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var conn = Connection;
            return await conn.ExecuteAsync("DeleteEmployee", new { Id = id }, commandType: CommandType.StoredProcedure);
        }
    }

}
