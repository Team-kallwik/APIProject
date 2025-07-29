using System.Text.Json.Serialization;

namespace DapperWebApiWIthAuthentication.Models
{
    public class Employee
    {
       
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
    }
}
