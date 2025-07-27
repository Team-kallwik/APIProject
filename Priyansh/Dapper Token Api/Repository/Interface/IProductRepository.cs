using Dapper_Token_Api.Dto;
using Dapper_Token_Api.Model;

namespace Dapper_Token_Api.Repository.Interface
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> CreateAsync(ProductCreateDto productDto, int userId);
        Task<bool> UpdateAsync(int id, ProductCreateDto productDto, int userId);
        Task<bool> DeleteAsync(int id, int userId);
    }
}