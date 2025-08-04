using Dapper_Token_Api.Dto;
using Dapper_Token_Api.Model;

namespace Dapper_Token_Api.Repository.Interface
{
    public interface IUserRepository : IGenericRepository<User>
    {
        // Keep your existing business-specific methods
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task<int> CreateAsync(RegisterDto registerDto);
        Task<bool> UpdateAsync(int id, User user);
        Task<bool> IsUsernameExistsAsync(string username);
        Task<bool> IsEmailExistsAsync(string email);
    }
}