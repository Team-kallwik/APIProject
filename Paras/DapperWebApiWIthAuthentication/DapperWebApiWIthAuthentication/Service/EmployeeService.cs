using DapperWebApiWIthAuthentication.Models;
using DapperWebApiWIthAuthentication.Repository;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DapperWebApiWIthAuthentication.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(IEmployeeRepository repository, ILogger<EmployeeService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeeAsync()
        {
            try
            {
                var jsonResult = await _repository.GetAllAsync();

                if (string.IsNullOrWhiteSpace(jsonResult))
                {
                    _logger.LogWarning("No employees found (empty JSON).");
                    return Enumerable.Empty<Employee>();
                }

                var employees = JsonSerializer.Deserialize<List<Employee>>(jsonResult);

                if (employees == null || !employees.Any())
                {
                    _logger.LogWarning("Deserialized list is empty.");
                    return Enumerable.Empty<Employee>();
                }

                _logger.LogInformation("Successfully retrieved {Count} employees.", employees.Count);
                return employees;
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "Failed to deserialize employee JSON.");
                throw new Exception("Failed to process employee data.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all employees.");
                throw new Exception("Error retrieving employees.");
            }
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            try
            {
                var employee = await _repository.GetByIdAsync(id);

                if (employee == null)
                {
                    _logger.LogWarning("Employee with ID {Id} not found.", id);
                    return null;
                }

                _logger.LogInformation("Employee with ID {Id} retrieved successfully.", id);
                return employee;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employee with ID {Id}.", id);
                throw new Exception("Error retrieving employee.");
            }
        }

        public async Task CreateEmployeeAsync(CreateEmployeeDto dto)
        {
            try
            {
                await _repository.CreateAsync(dto);
                _logger.LogInformation("Employee created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee.");
                throw new Exception("Failed to create employee.");
            }
        }

        public async Task<bool> UpdateEmployeeAsync(Employee employee)
        {
            try
            {
                var updated = await _repository.UpdateAsync(employee);

                if (!updated)
                {
                    _logger.LogWarning("Update failed. Employee with ID {Id} not found.", employee.Id);
                    return false;
                }

                _logger.LogInformation("Employee with ID {Id} updated successfully.", employee.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee with ID {Id}.", employee.Id);
                throw new Exception("Failed to update employee.");
            }
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            try
            {
                var deleted = await _repository.DeleteAsync(id);

                if (!deleted)
                {
                    _logger.LogWarning("Delete failed. Employee with ID {Id} not found.", id);
                    return false;
                }

                _logger.LogInformation("Employee with ID {Id} deleted successfully.", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee with ID {Id}.", id);
                throw new Exception("Failed to delete employee.");
            }
        }
    }
}
