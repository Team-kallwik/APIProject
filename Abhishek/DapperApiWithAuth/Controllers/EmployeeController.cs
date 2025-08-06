using DapperApiWithAuth.DTOs;
using DapperApiWithAuth.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DapperApiWithAuth.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/employees")]
    public class EmployeeController : ControllerBase
    {
        private IEmployeeRepository _employeeRepo;
        private ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeRepository employeeRepo, ILogger<EmployeeController> logger) 
        {
            _employeeRepo = employeeRepo;
            _logger = logger;
        }
        [HttpGet]

        // ActionResult<> allows you to return different kinds of HTTP responses 200 OK, 404 NotFound, 500 Server Error
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAll() 
        {
            try
            {
                var employees = await _employeeRepo.GetAllAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAll Employee Failed");
                return StatusCode(500, $"server error: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> Get(int id) 
        {
            try
            {
                var employee = await _employeeRepo.GetByIdAsync(id);
                if (employee == null) return NotFound();
                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get employee by id failed");
                return StatusCode(500, "server error");
            }
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> AddBulk([FromBody] IEnumerable<EmployeeDto> employees)
        {
            try
            {
                await _employeeRepo.AddBulkAsync(employees);
                return Ok("Employee Added");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Employee Added Failed");
                return StatusCode(500, $"Employee Added Failed: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(EmployeeDto emp)
        {
            try
            {
                await _employeeRepo.UpdateAsync(emp);
                return Ok("Employee Updated Successfull");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Employee Updated Failed");
                return StatusCode(500, "server error");
            }

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _employeeRepo.DeleteAsync(id);
                return Ok("Employee Deleted Succesfully");
            }catch(Exception ex)
            {
                _logger.LogError(ex, "Employee Deleted Failed");
                return StatusCode(500, "Server Error");
            }
        }
    }
}
