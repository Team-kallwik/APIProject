using DapperConfiguration.Models;
using DapperConfiguration.Repository;

namespace DapperConfiguration.Services
{
    public class EmploService : IEmploService
    {
        private readonly IEmploRepository _repository;

        public EmploService(IEmploRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Emplo>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Emplo> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<int> CreateAsync(Emplo employee)
        {
            return await _repository.CreateAsync(employee);
        }

        public async Task<int> UpdateAsync(Emplo employee)
        {
            return await _repository.UpdateAsync(employee);
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}