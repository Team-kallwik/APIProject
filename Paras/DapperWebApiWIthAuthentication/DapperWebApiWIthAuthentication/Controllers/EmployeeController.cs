using DapperWebApiWIthAuthentication.Models;
using DapperWebApiWIthAuthentication.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DapperWebApiWIthAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;

        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }

        [HttpGet("GetAllEmployee")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllEmployeeAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Error in GetAllEmployee: " + ex.Message);
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpGet("GetEmployeeById/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await _service.GetEmployeeByIdAsync(id);
                return result == null ? NotFound("Employee with ID not found.") : Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, " Internal server error: {ex.Message}");
            }
        }

        [HttpPost("CreateEmployee")]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
        {
            try
            {
                await _service.CreateEmployeeAsync(dto);
                return Ok(" Employee created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500 ,"Error creating employee: {ex.Message}");
            }
        }

        [HttpPut("UpdateEmployee")]
        public async Task<IActionResult> Update([FromBody] Employee employee)
        {
            try
            {
                await _service.UpdateEmployeeAsync(employee);
                return Ok(" Employee updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, " Error updating employee: {ex.Message}");
            }
        }

        [HttpDelete("DeleteEmployee/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteEmployeeAsync(id);
                return Ok(" Employee deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, " Error deleting employee: {ex.Message}");
            }
        }
    }
}
