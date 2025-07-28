using Dapper;
using DapperWebApiWIthAuthentication.Models;
using System.Data;

namespace DapperWebApiWIthAuthentication.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DapperContext _context;

        public EmployeeRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            try
            {
                using var connection = _context.CreateConnection();
                return await connection.QueryAsync<Employee>("GetAllEmployee", commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Error in GetAllAsync: " + ex.Message);
                return new List<Employee>();

            }
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            try
            {
                using var connection = _context.CreateConnection();
                return await connection.QueryFirstOrDefaultAsync<Employee>(
                    "GetEmployeeById", new { Id = id }, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Error in GetByIdAsync: " + ex.Message);
                return null;
            }
        }

        public async Task CreateAsync(CreateEmployeeDto dto)
        {
            try
            {
                using var connection = _context.CreateConnection();
                await connection.ExecuteAsync(
                    "CreateEmployee",
                    new
                    {
                        Name = dto.Name,
                        Email = dto.Email,
                        Department = dto.Department
                    },
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Error in CreateAsync: " + ex.Message);
            }
        }

        public async Task UpdateAsync(Employee employee)
        {
            try
            {
                using var connection = _context.CreateConnection();
                await connection.ExecuteAsync(
                    "UpdateEmployee",
                    new
                    {
                        Id = employee.Id,
                        Name = employee.Name,
                        Email = employee.Email,
                        Department = employee.Department
                    },
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Error in UpdateAsync: " + ex.Message);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var connection = _context.CreateConnection();
                await connection.ExecuteAsync(
                    "DeleteEmployee",
                    new { Id = id },
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Error in DeleteAsync: " + ex.Message);
            }
        }
    }
}
