using DapperAuthApi.Models;

namespace DapperAuthApi.Repository
{
    public interface IAuthRepository
    {
        Task<string> RegisterAsync(UserModel user);
        Task<string> LoginAsync(UserModel user);
    }
}