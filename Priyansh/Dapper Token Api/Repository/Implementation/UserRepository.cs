using Dapper;
using Dapper_Token_Api.Dto;
using Dapper_Token_Api.Model;
using Dapper_Token_Api.Repository.Interface;
using System.Data;

namespace Dapper_Token_Api.Repository.Implementation
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly IRepository _repository;

        public UserRepository(IRepository repository) : base(repository)
        {
            _repository = repository;
        }

        // Override generic methods to use stored procedures
        public override async Task<User?> GetByIdAsync(int id)
        {
            return await GetByIdUsingSpAsync(id, "[dbo].[sp_GetUserById]");
        }

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            return await GetAllUsingSpAsync("[dbo].[sp_GetAllUsers]");
        }

        // Keep your existing business-specific methods
        public async Task<User?> GetByUsernameAsync(string username)
        {
            var parameters = new { Username = username };
            return await _repository.QuerySingleOrDefaultAsync<User>(
                "[dbo].[sp_GetUserByUsername]",
                parameters,
                CommandType.StoredProcedure);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var parameters = new { Email = email };
            return await _repository.QuerySingleOrDefaultAsync<User>(
                "[dbo].[sp_GetUserByEmail]",
                parameters,
                CommandType.StoredProcedure);
        }

        public async Task<int> CreateAsync(RegisterDto registerDto)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var parameters = new DynamicParameters();
            parameters.Add("@Username", registerDto.Username, DbType.String, ParameterDirection.Input);
            parameters.Add("@Email", registerDto.Email, DbType.String, ParameterDirection.Input);
            parameters.Add("@PasswordHash", hashedPassword, DbType.String, ParameterDirection.Input);
            parameters.Add("@UserId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _repository.ExecuteAsync(
                "[dbo].[sp_RegisterUser]",
                parameters,
                CommandType.StoredProcedure);

            return parameters.Get<int>("@UserId");
        }

        public async Task<bool> UpdateAsync(int id, User user)
        {
            var parameters = new
            {
                Id = id,
                Username = user.Username,
                Email = user.Email,
                IsActive = user.IsActive
            };

            var rowsAffected = await _repository.ExecuteAsync(
                "[dbo].[sp_UpdateUser]",
                parameters,
                CommandType.StoredProcedure);

            return rowsAffected > 0;
        }

        // Override generic delete to use stored procedure
        public override async Task<bool> DeleteAsync(int id)
        {
            return await DeleteUsingSpAsync(id, "[dbo].[sp_DeleteUser]");
        }

        public async Task<bool> IsUsernameExistsAsync(string username)
        {
            var parameters = new { Username = username };
            var count = await _repository.ExecuteScalarAsync<int>(
                "[dbo].[sp_CheckUsernameExists]",
                parameters,
                CommandType.StoredProcedure);

            return count > 0;
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            var parameters = new { Email = email };
            var count = await _repository.ExecuteScalarAsync<int>(
                "[dbo].[sp_CheckEmailExists]",
                parameters,
                CommandType.StoredProcedure);

            return count > 0;
        }
    }
}