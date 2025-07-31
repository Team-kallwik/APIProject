using Dapper_Api_With_Token_Authentication.Helpers;
using Dapper_Api_With_Token_Authentication.Model;
using Dapper_Api_With_Token_Authentication.Repository.Interface;
using Dapper_Api_With_Token_Authentication.Services.Interface;
using Dapper_Api_With_Token_Authentication.Exceptions; // ✅ Import custom exceptions
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
        try
        {
            var jsonResult = await _repo.GetAllAsJsonAsync("GetAllEmployees");
            var employees = JsonSerializer.Deserialize<IEnumerable<Emp>>(jsonResult);

            if (employees == null || !employees.Any())
                throw new NotFoundException("No employees found in the database.");

            foreach (var emp in employees)
            {
                if (!string.IsNullOrEmpty(emp.Salary) && IsBase64String(emp.Salary))
                    emp.Salary = _encryptionHelper.Decrypt(emp.Salary);
            }

            return employees;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetAllAsync");
            throw;
        }
    }

    public async Task<Emp> GetByIdAsync(int id)
    {
        try
        {
            if (id <= 0)
                throw new ValidationException("Invalid employee ID. ID must be greater than zero.");

            var jsonParam = JsonSerializer.Serialize(new { Id = id });
            var employee = await _repo.GetByIdAsync("GetEmployeeById", new { JsonData = jsonParam });

            if (employee == null)
                throw new NotFoundException($"Employee with ID {id} not found.");

            if (!string.IsNullOrEmpty(employee.Salary) && IsBase64String(employee.Salary))
                employee.Salary = _encryptionHelper.Decrypt(employee.Salary);

            return employee;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetByIdAsync for ID {Id}", id);
            throw;
        }
    }

    public async Task<int> AddAsync(Emp emp)
    {
        try
        {
            // Validate inputs
            if (string.IsNullOrEmpty(emp.Name))
                throw new ValidationException("Employee name is required.");
            if (string.IsNullOrEmpty(emp.Department))
                throw new ValidationException("Employee department is required.");
            if (string.IsNullOrEmpty(emp.Salary))
                throw new ValidationException("Employee salary is required.");

            // Encrypt salary
            emp.Salary = _encryptionHelper.Encrypt(emp.Salary);

            var json = JsonSerializer.Serialize(new[] { emp });
            return await _repo.AddAsync("AddEmployeeFromJson", new { json });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in AddAsync for employee {@Emp}", emp);
            throw;
        }
    }

    public async Task<int> UpdateAsync(Emp emp)
    {
        try
        {
            if (emp.Id <= 0)
                throw new ValidationException("Invalid employee ID for update.");

            // Encrypt salary
            emp.Salary = _encryptionHelper.Encrypt(emp.Salary);

            var json = JsonSerializer.Serialize(new[] { emp });
            return await _repo.UpdateAsync("UpdateEmployeeFromJson", new { json });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateAsync for employee ID {Id}", emp.Id);
            throw;
        }
    }

    public async Task<int> DeleteAsync(int id)
    {
        try
        {
            if (id <= 0)
                throw new ValidationException("Invalid employee ID for deletion.");

            var rowsAffected = await _repo.DeleteAsync("DeleteEmployeeById", new { Id = id });

            if (rowsAffected == 0)
                throw new NotFoundException($"Employee with ID {id} could not be deleted or does not exist.");

            return rowsAffected;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in DeleteAsync for ID {Id}", id);
            throw;
        }
    }

    private bool IsBase64String(string input)
    {
        Span<byte> buffer = new Span<byte>(new byte[input.Length]);
        return Convert.TryFromBase64String(input, buffer, out _);
    }
}
