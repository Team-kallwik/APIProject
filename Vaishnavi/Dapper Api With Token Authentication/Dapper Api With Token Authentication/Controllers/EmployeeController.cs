using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Dapper_Api_With_Token_Authentication.Model;
using Dapper_Api_With_Token_Authentication.Services.Interface;

namespace Dapper_Api_With_Token_Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;

        public EmployeeController(IEmployeeService service) => _service = service;

        [HttpGet("GetAllEmployee")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching employee list: {ex.Message}");
            }
        }

        [HttpGet("GetEmployeeByID/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var employee = await _service.GetByIdAsync(id);
                if (employee == null)
                    return NotFound("Employee not found");

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching employee by ID: {ex.Message}");
            }
        }

        [HttpPost("CreateEmployee")]
        public async Task<IActionResult> Create([FromBody] Emp empDto)
        {
            try
            {
                var emp = new Emp
                {
                    Name = empDto.Name,
                    Department = empDto.Department,
                    Salary = empDto.Salary
                };

                var result = await _service.AddAsync(emp);

                if (result > 0)
                    return Ok(new { message = "Employee created successfully", emp });

                Console.WriteLine("⚠️ Stored procedure returned 0 rows.");
                return BadRequest("Employee creation failed");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error: " + ex.Message);
                return StatusCode(500, $"Error creating employee: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPut("UpdateEmployee/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Emp emp)
        {
            try
            {
                if (emp == null)
                    return BadRequest("Employee data is required.");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                emp.Id = id;

                var existing = await _service.GetByIdAsync(id);
                if (existing == null)
                    return NotFound($"No employee found with ID = {id}");

                var result = await _service.UpdateAsync(emp);

                if (result > 0)
                {
                    return Ok(new
                    {
                        message = "Employee updated successfully.",
                        updatedEmployee = emp
                    });
                }

                return BadRequest("Update failed. No rows were affected.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("DeleteEmployee/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);
                if (result > 0)
                    return Ok("Employee deleted successfully");

                return NotFound("Employee not found or could not be deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting employee: {ex.Message}");
            }
        }
    }
}
