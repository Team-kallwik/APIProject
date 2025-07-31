using Microsoft.AspNetCore.Mvc;
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
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService service, ILogger<EmployeeController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("GetAllEmployee")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("GetEmployeeByID/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _service.GetByIdAsync(id);
            return Ok(employee);
        }

        [HttpPost("CreateEmployee")]
        public async Task<IActionResult> Create([FromBody] Emp empDto)
        {
            var emp = new Emp
            {
                Name = empDto.Name,
                Department = empDto.Department,
                Salary = empDto.Salary
            };

            var result = await _service.AddAsync(emp);
            return Ok(new { message = "Employee created successfully", emp });
        }

        [HttpPut("UpdateEmployee/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Emp emp)
        {
            emp.Id = id;
            var result = await _service.UpdateAsync(emp);
            return Ok(new { message = "Employee updated successfully.", updatedEmployee = emp });
        }

        [HttpDelete("DeleteEmployee/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            return Ok("Employee deleted successfully");
        }
    }
}
