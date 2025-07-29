using Dapper;
using DapperWebApiWIthAuthentication.Models;
using DapperWebApiWIthAuthentication.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.OpenApi.Any;
using System.Linq.Expressions;

namespace DapperWebApiWIthAuthentication.Service
{
    public class EmployeeService :IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }
        public async Task <IEnumerable<Employee>>GetAllEmployeeAsync()
        {
            try
            {
                var AllEmployee = await _repository.GetAllAsync();

                if (AllEmployee==null&& AllEmployee.Any())
                {
                    throw new Exception("No Employee Found ");
                }
                return AllEmployee;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while fetching data"+ex.Message);
                throw new Exception("Something went wrong while retrieving  data");
            }
        }
        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            try
            {
                var employee = await _repository.GetByIdAsync(id);
                if (employee == null)
                {
                    Console.WriteLine(" Employee ID not found.");
                    return null;
                }
                return employee;
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Error getting employee by ID: " + ex.Message);
                return null; 
            }
        }
        public async Task CreateEmployeeAsync(CreateEmployeeDto dto)
        {
            try
            {
                await _repository.CreateAsync(dto);
                Console.WriteLine(" Employee created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Error while creating employee: " + ex.Message);
            }
        }
        public  async Task UpdateEmployeeAsync(Employee employee)
        {
            try
            {
                 await _repository.UpdateAsync(employee);
                Console.WriteLine("Update Employee Successfully ");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while updating Employee"+ex.Message);
            }
        }
        public  async Task DeleteEmployeeAsync(int id)
        {
            try
            {
                 await _repository.DeleteAsync(id);
                Console.WriteLine("Delete Employee Successfully");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error while Deleting Employee"+ex.Message);
            }
        }








    }
}
