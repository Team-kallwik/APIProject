using System.ComponentModel.DataAnnotations;

namespace DapperWebApiWIthAuthentication.Models
{
    public class RegisterEmployee
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

}
