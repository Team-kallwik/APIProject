using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text.Json;
using DapperAuthApi.Models;
using DapperAuthApi.Repository;

public class EmploRepository : IEmploRepository
{
    private readonly string _connectionString;
    public EmploRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection");
    }

    private IDbConnection Connection => new MySqlConnection(_connectionString);

    public async Task<IEnumerable<Emplo>> GetAllAsync()
    {
        try
        {
            using var conn = Connection;
            return await conn.QueryAsync<Emplo>("GetAllEmployees", commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            throw new Exception("Error fetching employees.", ex);
        }
    }

    public async Task<Emplo> GetByIdAsync(int id)
    {
        try
        {
            using var conn = Connection;
            string jsonParam = JsonSerializer.Serialize(new { Id = id });

            return await conn.QueryFirstOrDefaultAsync<Emplo>(
                "GetEmployeeByIdFromJson",
                new { empJson = jsonParam },
                commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error fetching employee with Id = {id}.", ex);
        }
    }

    public async Task<int> CreateAsync(Emplo emp)
    {
        try
        {
            using var conn = Connection;
            string jsonParam = JsonSerializer.Serialize(emp);

            return await conn.ExecuteAsync(
                "InsertEmployeeFromJson",
                new { empJson = jsonParam },
                commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            throw new Exception("Error inserting employee.", ex);
        }
    }

    public async Task<int> UpdateAsync(Emplo emp)
    {
        try
        {
            using var conn = Connection;
            string jsonParam = JsonSerializer.Serialize(emp);

            return await conn.ExecuteAsync(
                "UpdateEmployeeFromJson",
                new { empJson = jsonParam },
                commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error updating employee with Id = {emp.Id}.", ex);
        }
    }

    public async Task<int> DeleteAsync(int id)
    {
        try
        {
            using var conn = Connection;
            string jsonParam = JsonSerializer.Serialize(new { Id = id });

            return await conn.ExecuteAsync(
                "DeleteEmployeeFromJson",
                new { empJson = jsonParam },
                commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting employee with Id = {id}.", ex);
        }
    }
}
