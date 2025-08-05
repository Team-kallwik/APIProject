using Dapper;
using DapperAPI.Exceptions;
using DapperAPI.Model;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json;

namespace DapperAPI.Repositories
{
    public class CustomerRepository : IGenericRepository<Customer>
    {
        private readonly IDbConnection Connection;
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(IDbConnection connection, ILogger<CustomerRepository> logger)
        {
            Connection = connection;
            _logger = logger;
        }

        public async Task EnsureTableExistsAsync()
        {
            var EnsureTable = await Connection.ExecuteAsync("EnsureCustomerTableExists", commandType: CommandType.StoredProcedure);
            if (EnsureTable == 0)
                throw new NotFoundException();
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            var json = await Connection.QuerySingleOrDefaultAsync<string>(
                "GetAllCustomers",
                commandType: CommandType.StoredProcedure
            );
            if (json is null)
                throw new NotFoundException();
            return JsonSerializer.Deserialize<IEnumerable<Customer>>(json ?? "[]");
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new InvalidDetailsException();

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

            if (string.IsNullOrEmpty(json))
                throw new InvalidDetailsException();

            return await Connection.ExecuteAsync(
            "AddCustomerFromJson",
            new { Json = json },
            commandType: CommandType.StoredProcedure);

        }

        public async Task<int> UpdateAsync(Customer customer)
        {
            if (customer.CustId <= 0)
                throw new InvalidDetailsException();

            var json = JsonSerializer.Serialize(new[] { customer });
            var UpdateData = await Connection.ExecuteAsync
            (
                "UpdateCustomerFromJson",
                new { Json = json },
                commandType: CommandType.StoredProcedure
            );
            if (UpdateData == 0)
                throw new NotFoundException();
            return UpdateData;
        }

        public async Task<int> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new InvalidDetailsException();

            var DeleteData = await Connection.ExecuteAsync(
                "DeleteCustomerById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );

            if (DeleteData == 0)
                throw new NotFoundException();

            return DeleteData;
        }

    }

}