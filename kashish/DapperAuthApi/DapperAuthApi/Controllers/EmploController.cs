using DapperAuthApi.Models;
using DapperAuthApi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DapperAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmploController : ControllerBase
    {
        private readonly IEmploRepository _repo;
        private readonly ILogger<EmploController> _logger;

        public EmploController(IEmploRepository repo, ILogger<EmploController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet("GetAllEmployee")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("➡️ GET: GetAllEmployee called");

            try
            {
                var employees = await _repo.GetAllAsync();
                _logger.LogInformation("✅ Successfully fetched all employees. Count: {Count}", employees != null ? employees.Count() : 0);

                // _logger.LogInformation("✅ Successfully fetched all employees. Count: {Count}", employees?.Count);
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Exception in GetAllEmployee");
                return StatusCode(500, "An error occurred while retrieving employee data.");
            }
        }

        [HttpGet("GetEmployeeById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("➡️ GET: GetEmployeeById called with Id: {Id}", id);

            try
            {
                var emp = await _repo.GetByIdAsync(id);

                if (emp == null)
                {
                    _logger.LogWarning("⚠️ Employee not found with Id: {Id}", id);
                    return NotFound("Employee not found.");
                }

                _logger.LogInformation("✅ Found employee with Id: {Id}", id);
                return Ok(emp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Exception in GetEmployeeById for Id: {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the employee.");
            }
        }

        [HttpPost("CreateEmployee")]
        public async Task<IActionResult> Create([FromBody] Emplo emp)
        {
            _logger.LogInformation("➡️ POST: CreateEmployee called for Name: {Name}", emp.Name);

            try
            {
                var result = await _repo.CreateAsync(emp);

                if (result > 0)
                {
                    _logger.LogInformation("✅ Employee created: {Name}", emp.Name);
                    return Ok("Inserted");
                }

                _logger.LogWarning("⚠️ Failed to insert employee: {Name}", emp.Name);
                return BadRequest("Failed to insert employee.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Exception in CreateEmployee for Name: {Name}", emp.Name);
                return StatusCode(500, "An error occurred while creating the employee.");
            }
        }

        [HttpPut("UpdateEmployee")]
        public async Task<IActionResult> Update([FromBody] Emplo emp)
        {
            _logger.LogInformation("➡️ PUT: UpdateEmployee called for Id: {Id}", emp.Id);

            try
            {
                var result = await _repo.UpdateAsync(emp);

                if (result > 0)
                {
                    _logger.LogInformation("✅ Employee updated. Id: {Id}", emp.Id);
                    return Ok("Updated");
                }

                _logger.LogWarning("⚠️ No employee found to update. Id: {Id}", emp.Id);
                return NotFound("Employee not found for update.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Exception in UpdateEmployee for Id: {Id}", emp.Id);
                return StatusCode(500, "An error occurred while updating the employee.");
            }
        }

        [HttpDelete("DeleteEmployee/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("➡️ DELETE: DeleteEmployee called for Id: {Id}", id);

            try
            {
                var result = await _repo.DeleteAsync(id);

                if (result > 0)
                {
                    _logger.LogInformation("✅ Employee deleted. Id: {Id}", id);
                    return Ok("Deleted");
                }

                _logger.LogWarning("⚠️ No employee found to delete. Id: {Id}", id);
                return NotFound("Employee not found for deletion.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Exception in DeleteEmployee for Id: {Id}", id);
                return StatusCode(500, "An error occurred while deleting the employee.");
            }
        }
    }
}