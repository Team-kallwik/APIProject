using Dapper;
using DapperAPI.Model;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json;

namespace DapperAPI.Repositories
{
    public class CustomerRepository : ICustomer<Customer>
    {
        private readonly IDbConnection Connection;

        public CustomerRepository(IDbConnection connection)
        {
            Connection = connection;
        }

        public async Task EnsureTableExistsAsync()
        {
            var sql = @"-- use same table creation SQL here";
            await Connection.ExecuteAsync(sql);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            var json = await Connection.QuerySingleOrDefaultAsync<string>(
                "GetAllCustomers",
                commandType: CommandType.StoredProcedure
            );

            return JsonSerializer.Deserialize<IEnumerable<Customer>>(json ?? "[]");
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            var json = await Connection.QuerySingleOrDefaultAsync<string>(
                "GetCustomerById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );

            return json is null ? null : JsonSerializer.Deserialize<Customer>(json);
        }

        public async Task<int> AddAsync(Customer customer)
        {
            var json = JsonSerializer.Serialize(new[] { customer });
            return await Connection.ExecuteAsync(
                "AddCustomerFromJson",
                new { Json = json },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> UpdateAsync(Customer customer)
        {
            var json = JsonSerializer.Serialize(new[] { customer });
            return await Connection.ExecuteAsync(
                "UpdateCustomerFromJson",
                new { Json = json },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await Connection.ExecuteAsync(
                "DeleteCustomerById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }
    }

}