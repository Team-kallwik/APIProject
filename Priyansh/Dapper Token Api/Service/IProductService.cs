using Dapper_Token_Api.Dto;
using Dapper_Token_Api.Model;

namespace Dapper_Token_Api.Service
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(ProductCreateDto productDto, int userId);
        Task<bool> UpdateProductAsync(int id, ProductCreateDto productDto, int userId);
        Task<bool> DeleteProductAsync(int id, int userId);

    }
}