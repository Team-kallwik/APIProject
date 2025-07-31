using DapperApiWithAuth.Models;

namespace DapperApiWithAuth.Interfaces
{
    public interface IUserRepository
    {
        Task RegisterAsync(User user);
        Task<User> GetByUsernameAsync(string username);
    }
}
