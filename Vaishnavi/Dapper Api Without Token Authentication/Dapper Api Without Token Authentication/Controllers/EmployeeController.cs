using Dapper_Api_Without_Token_Authentication.Models;
using Dapper_Api_Without_Token_Authentication.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dapper_Api_Without_Token_Authentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;

        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                return result == null ? NotFound() : Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] Employee employee)
        {
            try
            {
                var result = await _service.AddAsync(employee);
                return Ok(new { Success = result > 0, Message = result > 0 ? "Employee added successfully" : "Failed to add employee" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Employee employee)
        {
            try
            {
                // Trust the URL ID and override body ID
                employee.Id = id;

                var result = await _service.UpdateAsync(employee);

                if (result > 0)
                    return Ok(new { Success = true, Message = "Employee updated successfully." });

                return NotFound(new { Success = false, Message = "Employee not found or update failed." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = $"Error: {ex.Message}" });
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);
                return Ok(new { Success = result > 0 });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
