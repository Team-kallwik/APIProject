using Microsoft.AspNetCore.Mvc;
using DapperConfiguration.Models;
using DapperConfiguration.Repository;


namespace DapperConfiguration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmploController : Controller
    {
        private readonly IEmploRepository _repository;

        public EmploController(IEmploRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("GetAllEmployee")]
        public async Task<IActionResult> GetAll() =>
            Ok(await _repository.GetAllAsync());

        [HttpGet("GetEmployeeById,{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var emp = await _repository.GetByIdAsync(id);
            return emp == null ? NotFound() : Ok(emp);
        }

        [HttpPost("CreateEmployee")]
        public async Task<IActionResult> Create([FromBody] Emplo employee)
        {
            await _repository.CreateAsync(employee);
            return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
        }

        [HttpPut("UpdateEmployee")]
        public async Task<IActionResult> Update(int id, [FromBody] Emplo employee)
        {
            if (id != employee.Id) return BadRequest();
            await _repository.UpdateAsync(employee);
            return NoContent();
        }

        [HttpDelete("DeleteEmployee,{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }

}

