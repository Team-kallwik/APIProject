using Dapper;
using Dapper_Token_Api.Dto;
using Dapper_Token_Api.Model;
using Dapper_Token_Api.Repository.Interface;
using System.Data;

namespace Dapper_Token_Api.Repository.Implementation
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly IRepository _repository;

        public ProductRepository(IRepository repository) : base(repository)
        {
            _repository = repository;
        }

        // Override generic methods to use stored procedures
        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await GetAllUsingSpAsync("[dbo].[sp_GetAllProducts]");
        }

        public override async Task<Product?> GetByIdAsync(int id)
        {
            return await GetByIdUsingSpAsync(id, "[dbo].[sp_GetProductById]");
        }

        // Keep your existing business-specific methods
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