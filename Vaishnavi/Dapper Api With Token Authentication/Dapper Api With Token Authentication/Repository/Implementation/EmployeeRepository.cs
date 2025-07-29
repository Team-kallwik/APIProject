using Dapper;
using Dapper_Api_With_Token_Authentication.Model;
using Dapper_Api_With_Token_Authentication.Repository.Interface;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json;

namespace Dapper_Api_With_Token_Authentication.Repository.Imp
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmployeeRepository> _logger;

        public EmployeeRepository(IConfiguration config, ILogger<EmployeeRepository> logger)
        {
            _config = config;
            _logger = logger;
        }

        private IDbConnection Connection => new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        public async Task<IEnumerable<Emp>> GetAllAsync()
        {
            _logger.LogInformation("Repo: Executing SP GetAllEmployees");
            using var db = Connection;
            return await db.QueryAsync<Emp>("GetAllEmployees", commandType: CommandType.StoredProcedure);
        }

        public async Task<Emp> GetByIdAsync(int id)
        {
            _logger.LogInformation("Repo: Executing SP GetEmployeeById with ID: {Id}", id);
            using var db = Connection;
            return await db.QueryFirstOrDefaultAsync<Emp>("GetEmployeeById", new { Id = id }, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> AddAsync(Emp emp)
        {
            _logger.LogInformation("Repo: Adding employee {@Employee}", emp);
            using var db = Connection;
            var json = JsonSerializer.Serialize(new[] { emp });

            var parameters = new DynamicParameters();
            parameters.Add("json", json);
            parameters.Add("Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await db.ExecuteAsync("AddEmployeeFromJson", parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("Result");
        }

        public async Task<int> UpdateAsync(Emp emp)
        {
            _logger.LogInformation("Repo: Updating employee {@Employee}", emp);
            using var db = Connection;
            var json = JsonSerializer.Serialize(new[] { emp });

            var parameters = new DynamicParameters();
            parameters.Add("json", json);
            parameters.Add("Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await db.ExecuteAsync("UpdateEmployeeFromJson", parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("Result");
        }

        public async Task<int> DeleteAsync(int id)
        {
            _logger.LogInformation("Repo: Deleting employee with ID {Id}", id);
            using var db = Connection;
            return await db.ExecuteAsync("DeleteEmployeeById", new { Id = id }, commandType: CommandType.StoredProcedure);
        }
    }
}
