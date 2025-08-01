using Dapper;
using DapperAPI.Data;
using DapperAPI.Exceptions;
using DapperAPI.Model;
using DapperAPI.Models;
using DapperAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DapperAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly IGenericRepository<Customer> _repository;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(IGenericRepository<Customer> repository, ILogger<CustomersController> logger)
        {
            _repository = repository;
            _logger = logger;
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
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, "Table not found during retrieval of all customers.");
                return NotFound("Table not found.");
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
                return Ok(customer);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, "Table not found during retrieval of customer with ID {Id}", id);
                return NotFound("Table not found.");
            }
            catch (InvalidDetailsException ex)
            {
                _logger.LogError(ex, "Invalid customer ID provided: {Id}", id);
                return BadRequest("Invalid customer ID provided.");
            }
            
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving customer with ID {Id}", id);
                return StatusCode(500, $"[GetById ERROR]: {ex.Message}");
            }
        }

        // POST: api/customers
        [HttpPost]
        public async Task<IActionResult> AddUser(Customer customer)
        {
            try
            {
                if (customer == null)
                    return BadRequest("Customer data is null.");                        
                var result = await _repository.AddAsync(customer);
                return result > 0
                    ? Ok("Customer added successfully.")
                    : BadRequest("Failed to add customer.");
            }
            catch (InvalidDetailsException ex)
            {
                _logger.LogError(ex, "Invalid customer details provided for addition.");
                return BadRequest("Invalid customer details provided.");
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, "Table not found during customer addition.");
                return NotFound("Table not found.");
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
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, "Table not found during customer update.");
                return NotFound("Table not found.");
            }
            catch (InvalidDetailsException ex)
            {
                _logger.LogError(ex, "Invalid customer details provided for update.");
                return BadRequest("Invalid customer details provided.");
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
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, "Table not found during customer deletion.");
                return NotFound("Table not found.");
            }
            catch (InvalidDetailsException ex)
            {
                _logger.LogError(ex, "Invalid customer ID provided for deletion: {Id}", id);
                return BadRequest("Invalid customer ID provided.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting customer with ID {Id}", id);
                return StatusCode(500, $"[Delete ERROR]: {ex.Message}");
            }
        }
    }
}
