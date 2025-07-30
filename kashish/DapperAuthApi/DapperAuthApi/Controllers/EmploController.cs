using DapperAuthApi.Models;
using DapperAuthApi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DapperAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmploController : Controller
    {
        private readonly IEmploRepository _repo;

        public EmploController(IEmploRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("GetAllEmployee")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var employees = await _repo.GetAllAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching employees.", Error = ex.Message });
            }
        }

        [HttpGet("GetEmployeeById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var emp = await _repo.GetByIdAsync(id);
                return emp == null ? NotFound(new { Message = "Employee not found." }) : Ok(emp);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching employee.", Error = ex.Message });
            }
        }

        [HttpPost("CreateEmployee")]
        public async Task<IActionResult> Create(Emplo emp)
        {
            try
            {
                var result = await _repo.CreateAsync(emp);
                return result > 0 ? Ok("Inserted") : BadRequest("Insertion failed.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error creating employee.", Error = ex.Message });
            }
        }

        [HttpPut("UpdateEmployee")]
        public async Task<IActionResult> Update(Emplo emp)
        {
            try
            {
                var result = await _repo.UpdateAsync(emp);
                return result > 0 ? Ok("Updated") : NotFound(new { Message = "Employee not found or update failed." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error updating employee.", Error = ex.Message });
            }
        }

        [HttpDelete("DeleteEmployee/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _repo.DeleteAsync(id);
                return result > 0 ? Ok("Deleted") : NotFound(new { Message = "Employee not found or delete failed." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error deleting employee.", Error = ex.Message });
            }
        }
    }
}