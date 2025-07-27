using Dapper_Token_Api.Dto;
using Dapper_Token_Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dapper_Token_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all products");
                return StatusCode(500, new { message = "Error retrieving products" });
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                    return NotFound(new { message = "Product not found" });

                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product with ID: {ProductId}", id);
                return StatusCode(500, new { message = "Error retrieving product" });
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto productDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var product = await _productService.CreateProductAsync(productDto, userId);
                _logger.LogInformation("Product created with ID: {ProductId} by user: {UserId}", product.Id, userId);
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                return StatusCode(500, new { message = "Error creating product" });
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductCreateDto productDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var success = await _productService.UpdateProductAsync(id, productDto, userId);
                if (!success)
                    return NotFound(new { message = "Product not found or you don't have permission to update it" });

                _logger.LogInformation("Product updated with ID: {ProductId} by user: {UserId}", id, userId);

                // Changed from NoContent() to Ok() with success message
                return Ok(new
                {
                    message = "Product successfully updated",
                    updatedAt = DateTime.UtcNow
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product with ID: {ProductId}", id);
                return StatusCode(500, new { message = "Error updating product" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var success = await _productService.DeleteProductAsync(id, userId);

                if (!success)
                    return NotFound(new { message = "Product not found or you don't have permission to delete it" });

                _logger.LogInformation("Product deleted with ID: {ProductId} by user: {UserId}", id, userId);
                return Ok(new
                {
                    message = "Product successfully deleted",
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product with ID: {ProductId}", id);
                return StatusCode(500, new { message = "Error deleting product" });
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("Invalid user token");
            }
            return userId;
        }
    }
}