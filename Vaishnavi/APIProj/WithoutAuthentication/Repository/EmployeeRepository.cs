using Dapper;
using DapperConfig.Repository;
using Microsoft.Data.SqlClient;
using DapperConfig.Models;
using System.Data;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public EmployeeRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection");
    }

    private IDbConnection Connection => new SqlConnection(_connectionString);

    public async Task<IEnumerable<Emp>> GetAllAsync()
    {
        var sql = "SELECT * FROM Emp";
        using var conn = Connection;
        return await conn.QueryAsync<Emp>(sql);
    }

    public async Task<Emp> GetByIdAsync(int id)
    {
        var sql = "SELECT * FROM Emp WHERE Id = @Id";
        using var conn = Connection;
        return await conn.QueryFirstOrDefaultAsync<Emp>(sql, new { Id = id });
    }

    public async Task<int> AddAsync(Emp employee)
    {
        var sql = "INSERT INTO Emp (Name, Department, Salary) VALUES (@Name, @Department, @Salary)";
        using var conn = Connection;
        return await conn.ExecuteAsync(sql, employee);
    }

    public async Task<int> UpdateAsync(Emp employee)
    {
        var sql = "UPDATE Emp SET Name = @Name, Department = @Department, Salary = @Salary WHERE Id = @Id";
        using var conn = Connection;
        return await conn.ExecuteAsync(sql, employee);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var sql = "DELETE FROM Emp WHERE Id = @Id";
        using var conn = Connection;
        return await conn.ExecuteAsync(sql, new { Id = id });
    }
}
