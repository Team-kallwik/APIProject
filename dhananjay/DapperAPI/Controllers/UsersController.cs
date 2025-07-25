using Dapper;
using DapperAPI.Data;
using DapperAPI.Model;
using DapperAPI.Models;
using DapperAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DapperAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerRepository _repository;

        public CustomersController(CustomerRepository repository)
        {
            _repository = repository;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var customers = await _repository.GetAllAsync();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"[GetAll ERROR]: {ex.Message}");
            }
        }

        // GET: api/customers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var customer = await _repository.GetByIdAsync(id);
                if (customer == null)
                    return NotFound("Customer not found.");
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"[GetById ERROR]: {ex.Message}");
            }
        }

        // POST: api/customers
        [HttpPost]
        public async Task<IActionResult> Add(Customer customer)
        {
            try
            {
                var result = await _repository.AddAsync(customer);
                return result > 0
                    ? Ok("Customer added successfully.")
                    : BadRequest("Failed to add customer.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"[Add ERROR]: {ex.Message}");
            }
        }

        // PUT: api/customers
        [HttpPut]
        public async Task<IActionResult> Update(Customer customer)
        {
            try
            {
                var result = await _repository.UpdateAsync(customer);
                return result > 0
                    ? Ok("Customer updated successfully.")
                    : NotFound("Customer not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"[Update ERROR]: {ex.Message}");
            }
        }

        // DELETE: api/customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _repository.DeleteAsync(id);
                return result > 0
                    ? Ok("Customer deleted successfully.")
                    : NotFound("Customer not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"[Delete ERROR]: {ex.Message}");
            }
        }
    }
}
