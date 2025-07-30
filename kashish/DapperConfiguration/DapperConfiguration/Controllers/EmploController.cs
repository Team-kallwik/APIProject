using DapperConfiguration.Models;
using DapperConfiguration.Services;
using Microsoft.AspNetCore.Mvc;

namespace DapperConfiguration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmploController : Controller
    {
        private readonly IEmploService _service;

        public EmploController(IEmploService service)
        {
            _service = service;
        }

        [HttpGet("GetAllEmployee")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Emplo emp)
        {
            var result = await _service.CreateAsync(emp);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] Emplo emp)
        {
            var result = await _service.UpdateAsync(emp);
            return Ok(result);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            return Ok(result);
        }
    }
}
