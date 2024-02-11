using System.ComponentModel.DataAnnotations;

namespace API.Application.DTOs.Registeration
{
    public class LoginDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
