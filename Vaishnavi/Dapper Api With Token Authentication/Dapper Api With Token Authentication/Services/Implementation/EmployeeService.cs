using Dapper_Api_With_Token_Authentication.Helpers;
using Dapper_Api_With_Token_Authentication.Model;
using Dapper_Api_With_Token_Authentication.Repository.Interface;
using Dapper_Api_With_Token_Authentication.Services.Interface;
using System.Text.Json;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repo;
    private readonly ILogger<EmployeeService> _logger;
    private readonly AesEncryptionHelper _encryptionHelper;

    public EmployeeService(IEmployeeRepository repo, ILogger<EmployeeService> logger, AesEncryptionHelper encryptionHelper)
    {
        _repo = repo;
        _logger = logger;
        _encryptionHelper = encryptionHelper;
    }

    public async Task<IEnumerable<Emp>> GetAllAsync()
    {
        var jsonResult = await _repo.GetAllAsJsonAsync("GetAllEmployees");
        var employees = JsonSerializer.Deserialize<IEnumerable<Emp>>(jsonResult);

        foreach (var emp in employees)
        {
            if (!string.IsNullOrEmpty(emp.Salary))
            {
                if (IsBase64String(emp.Salary)) // Decrypt if encrypted
                    emp.Salary = _encryptionHelper.Decrypt(emp.Salary);
                // else keep plain salary as is (already string)
            }
        }

        return employees;
    }

    public async Task<Emp> GetByIdAsync(int id)
    {
        var jsonParam = JsonSerializer.Serialize(new { Id = id });
        var employee = await _repo.GetByIdAsync("GetEmployeeById", new { JsonData = jsonParam });

        if (employee != null && !string.IsNullOrEmpty(employee.Salary))
        {
            if (IsBase64String(employee.Salary))
                employee.Salary = _encryptionHelper.Decrypt(employee.Salary);
        }

        return employee;
    }


    private bool IsBase64String(string input)
    {
        Span<byte> buffer = new Span<byte>(new byte[input.Length]);
        return Convert.TryFromBase64String(input, buffer, out _);
    }


    public async Task<int> AddAsync(Emp emp)
    {
        // Encrypt salary (convert to string first)
        emp.Salary = _encryptionHelper.Encrypt(emp.Salary);
        var json = JsonSerializer.Serialize(new[] { emp });
        return await _repo.AddAsync("AddEmployeeFromJson", new { json });
    }

    public async Task<int> UpdateAsync(Emp emp)
    {
        emp.Salary = _encryptionHelper.Encrypt(emp.Salary);
        var json = JsonSerializer.Serialize(new[] { emp });
        return await _repo.UpdateAsync("UpdateEmployeeFromJson", new { json });
    }
    public async Task<int> DeleteAsync(int id)
    {
        return await _repo.DeleteAsync("DeleteEmployeeById", new { Id = id });
    }
}
