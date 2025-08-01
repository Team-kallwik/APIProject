using DapperAPI.DTOs;

namespace DapperAPI.Service
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(UserDto request);
        Task<string> LoginAsync(UserDto request);
    }
}
