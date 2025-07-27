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

        public EmployeeRepository(IConfiguration config) => _config = config;

        private IDbConnection Connection => new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        public async Task<IEnumerable<Emp>> GetAllAsync()
        {
            using var db = Connection;
            return await db.QueryAsync<Emp>("GetAllEmployees", commandType: CommandType.StoredProcedure);
        }

        public async Task<Emp> GetByIdAsync(int id)
        {
            using var db = Connection;
            return await db.QueryFirstOrDefaultAsync<Emp>("GetEmployeeById", new { Id = id }, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> AddAsync(Emp emp)
        {
            using var db = Connection;

            var json = JsonSerializer.Serialize(new[] { emp });

            var parameters = new DynamicParameters();
            parameters.Add("json", json);
            parameters.Add("Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await db.ExecuteAsync("AddEmployeeFromJson", parameters, commandType: CommandType.StoredProcedure);

            int result = parameters.Get<int>("Result");
            return result;
        }


        public async Task<int> UpdateAsync(Emp emp)
        {
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
            using var db = Connection;
            return await db.ExecuteAsync("DeleteEmployeeById", new { Id = id }, commandType: CommandType.StoredProcedure);
        }

    }

}