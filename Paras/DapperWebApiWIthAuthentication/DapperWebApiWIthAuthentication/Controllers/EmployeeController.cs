using DapperWebApiWIthAuthentication.Models;
using DapperWebApiWIthAuthentication.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DapperWebApiWIthAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService service, ILogger<EmployeeController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("GetAllEmployee")]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var employees = await _service.GetAllEmployeeAsync();

                if (employees == null || !employees.Any())
                {
                    _logger.LogWarning("No employees found.");
                    return NotFound(new { message = "No employees found." });
                }

                _logger.LogInformation("Successfully fetched all employees.");
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching employees.");
                return StatusCode(500, new { message = "Internal server error: " + ex.Message });
            }
        }

        [HttpGet("GetEmployeeById/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await _service.GetEmployeeByIdAsync(id);

                if (result == null)
                {
                    _logger.LogWarning("Employee with ID {Id} not found.", id);
                    return NotFound($"Employee with ID {id} not found.");
                }

                _logger.LogInformation("Employee with ID {Id} retrieved successfully.", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employee with ID {Id}.", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("CreateEmployee")]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid data.");

            try
            {
                await _service.CreateEmployeeAsync(dto);
                _logger.LogInformation("Employee created successfully.");
                return Ok("Employee created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee.");
                return StatusCode(500, $"Error creating employee: {ex.Message}");
            }
        }

        [HttpPut("UpdateEmployee")]
        public async Task<IActionResult> Update([FromBody] Employee employee)
        {
            if (employee == null)
                return BadRequest("Invalid employee data.");

            try
            {
                var result = await _service.UpdateEmployeeAsync(employee);

                if (!result)
                {
                    _logger.LogWarning("Employee with ID {EmployeeId} not found for update.", employee.Id);
                    return NotFound($"Employee with ID {employee.Id} not found.");
                }

                _logger.LogInformation("Employee with ID {EmployeeId} updated successfully.", employee.Id);
                return Ok("Employee updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee with ID {EmployeeId}.", employee.Id);
                return StatusCode(500, $"Error updating employee: {ex.Message}");
            }
        }

        [HttpDelete("DeleteEmployee/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.DeleteEmployeeAsync(id);

                if (!result)
                {
                    _logger.LogWarning("Employee with ID {Id} not found for delete.", id);
                    return NotFound($"Employee with ID {id} not found.");
                }

                _logger.LogInformation("Employee with ID {Id} deleted successfully.", id);
                return Ok("Employee deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee with ID {Id}.", id);
                return StatusCode(500, $"Error deleting employee: {ex.Message}");
            }
        }
    }
}
