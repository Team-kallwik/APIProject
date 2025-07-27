using Dapper;
using Dapper_Token_Api.Dto;
using Dapper_Token_Api.Model;
using Dapper_Token_Api.Repository.Interface;
using System.Data;

namespace Dapper_Token_Api.Repository.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly IRepository _repository;

        public ProductRepository(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _repository.QueryAsync<Product>(
                "[dbo].[sp_GetAllProducts]",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var parameters = new { Id = id };
            return await _repository.QuerySingleOrDefaultAsync<Product>(
                "[dbo].[sp_GetProductById]",
                parameters,
                CommandType.StoredProcedure);
        }

        public async Task<Product> CreateAsync(ProductCreateDto productDto, int userId)
        {
            var parameters = new
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                CreatedBy = userId
            };

            return await _repository.QuerySingleAsync<Product>(
                "[dbo].[sp_CreateProduct]",
                parameters,
                CommandType.StoredProcedure);
        }

        public async Task<bool> UpdateAsync(int id, ProductCreateDto productDto, int userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Name", productDto.Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@Description", productDto.Description, DbType.String, ParameterDirection.Input);
            parameters.Add("@Price", productDto.Price, DbType.Decimal, ParameterDirection.Input);
            parameters.Add("@CreatedBy", userId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@RowsAffected", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _repository.ExecuteAsync(
                "[dbo].[sp_UpdateProduct]",
                parameters,
                CommandType.StoredProcedure);

            return parameters.Get<int>("@RowsAffected") > 0;
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@CreatedBy", userId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@RowsAffected", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _repository.ExecuteAsync(
                "[dbo].[sp_DeleteProduct]",
                parameters,
                CommandType.StoredProcedure);

            return parameters.Get<int>("@RowsAffected") > 0;
        }
    }
}