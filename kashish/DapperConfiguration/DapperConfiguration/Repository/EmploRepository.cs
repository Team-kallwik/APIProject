using Dapper;
using DapperConfiguration.Models;
using DapperConfiguration.Repository;
using MySql.Data.MySqlClient;
using System.Data;

public class EmploRepository : IEmploRepository
{
    private readonly string _connectionString;

    public EmploRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    private IDbConnection Connection => new MySqlConnection(_connectionString);

    public async Task<IEnumerable<Emplo>> GetAllAsync()
    {
        var query = "SELECT * FROM Emplo";
        using var db = Connection;
        return await db.QueryAsync<Emplo>(query);
    }

    public async Task<Emplo> GetByIdAsync(int id)
    {
        var query = "SELECT * FROM Emplo WHERE Id = @Id";
        using var db = Connection;
        return await db.QueryFirstOrDefaultAsync<Emplo>(query, new { Id = id });
    }

    public async Task<int> CreateAsync(Emplo employee)
    {
        var query = @"INSERT INTO Emplo (Name, Department, Salary )
                      VALUES (@Name, @Department, @Salary)";
        using var db = Connection;
        return await db.ExecuteAsync(query, employee);
    }

    public async Task<int> UpdateAsync(Emplo employee)
    {
        var query = @"UPDATE Emplo SET 
                      Name = @Name, Department = @Department, 
                      Salary = @Salary
                      WHERE Id = @Id";
        using var db = Connection;
        return await db.ExecuteAsync(query, employee);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var query = "DELETE FROM Emplo WHERE Id = @Id";
        using var db = Connection;
        return await db.ExecuteAsync(query, new { Id = id });
    }
}