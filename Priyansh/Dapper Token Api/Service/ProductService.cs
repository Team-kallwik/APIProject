using Dapper_Token_Api.Dto;
using Dapper_Token_Api.Model;
using Dapper_Token_Api.Repository.Interface;

namespace Dapper_Token_Api.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<Product> CreateProductAsync(ProductCreateDto productDto, int userId)
        {
            ValidateProductDto(productDto);
            return await _productRepository.CreateAsync(productDto, userId);
        }

        public async Task<bool> UpdateProductAsync(int id, ProductCreateDto productDto, int userId)
        {
            ValidateProductDto(productDto);
            return await _productRepository.UpdateAsync(id, productDto, userId);
        }

        public async Task<bool> DeleteProductAsync(int id, int userId)
        {
            return await _productRepository.DeleteAsync(id, userId);
        }

        private void ValidateProductDto(ProductCreateDto productDto)
        {
            if (string.IsNullOrWhiteSpace(productDto.Name))
            {
                throw new ArgumentException("Product name is required");
            }

            if (productDto.Price < 0)
            {
                throw new ArgumentException("Product price cannot be negative");
            }
        }
    }
}