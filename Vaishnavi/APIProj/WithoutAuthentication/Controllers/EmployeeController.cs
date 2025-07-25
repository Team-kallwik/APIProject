using Microsoft.AspNetCore.Mvc;
using DapperConfig.Models;
using DapperConfig.Repository;

namespace DapperConfig.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return Ok(employees);
        }

        [HttpGet("GetEmployeeByID,{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return NotFound();
            return Ok(employee);
        }

        [HttpPost("CreateEmployee")]
        public async Task<IActionResult> Create(Emp employee)
        {
            await _employeeRepository.AddAsync(employee);
            return Ok(new { message = "Employee added successfully" });
        }

        [HttpPut("UpdateEmployee")]
        public async Task<IActionResult> Update(Emp employee)
        {
            await _employeeRepository.UpdateAsync(employee);
            return Ok(new { message = "Employee updated successfully" });
        }
        [HttpDelete("DeleteEmployee/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _employeeRepository.DeleteAsync(id);

            if (result == 0)
            {
                return NotFound(new { message = "Please enter a valid ID. Employee does not exist." });
            }

            return Ok(new { message = "Employee deleted successfully" });
        }

    }
}

