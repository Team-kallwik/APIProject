using Dapper_Api_With_Token_Authentication.Model;
using Dapper_Api_With_Token_Authentication.Repository.Interface;
using Microsoft.Extensions.Logging;

namespace Dapper_Api_With_Token_Authentication.Repository.Imp
{
    public class EmployeeRepository : GenericRepository<Emp>, IEmployeeRepository
    {
        private readonly ILogger<EmployeeRepository> _logger;

        public EmployeeRepository(
            IConfiguration config,
            ILogger<EmployeeRepository> logger,
            ILogger<GenericRepository<Emp>> genericLogger)
            : base(config, genericLogger)
        {
            _logger = logger;
        }
    }
}
