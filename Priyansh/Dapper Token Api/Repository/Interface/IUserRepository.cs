using Dapper_Token_Api.Dto;
using Dapper_Token_Api.Model;

namespace Dapper_Token_Api.Repository.Interface
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task<int> CreateAsync(RegisterDto registerDto);
        Task<bool> UpdateAsync(int id, User user);
        Task<bool> DeleteAsync(int id);
        Task<bool> IsUsernameExistsAsync(string username);
        Task<bool> IsEmailExistsAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
    }
}
