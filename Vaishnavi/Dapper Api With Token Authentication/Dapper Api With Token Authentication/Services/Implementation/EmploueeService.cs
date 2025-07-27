using Dapper_Api_With_Token_Authentication.Model;
using Dapper_Api_With_Token_Authentication.Repository.Interface;
using Dapper_Api_With_Token_Authentication.Services.Interface;

namespace Dapper_Api_With_Token_Authentication.Services.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repo;

        public EmployeeService(IEmployeeRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Emp>> GetAllAsync()
        {
            try
            {
                return await _repo.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting all employees", ex);
            }
        }

        public async Task<Emp> GetByIdAsync(int id)
        {
            try
            {
                return await _repo.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting employee by ID", ex);
            }
        }

        public async Task<int> AddAsync(Emp emp)
        {
            try
            {
                return await _repo.AddAsync(emp);
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding employee", ex);
            }
        }

        public async Task<int> UpdateAsync(Emp emp)
        {
            try
            {
                return await _repo.UpdateAsync(emp);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating employee", ex);
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            try
            {
                return await _repo.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting employee", ex);
            }
        }
    }
}