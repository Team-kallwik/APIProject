using Dapper_Token_Api.Dto;
using Dapper_Token_Api.Model;

namespace Dapper_Token_Api.Service
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<string> RegisterAsync(RegisterDto registerDto);
        Task<User?> GetUserByUsernameAsync(string username);
        string GenerateJwtToken(User user);
    }
}