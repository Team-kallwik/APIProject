namespace DapperAuthApi.Services
{
    public interface IJwtAuthManager
    {
       
            string GenerateToken(string username);
        
    }
}
