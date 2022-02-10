using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class AccountDTO : UserDTO
    {
        [EmailAddress, Required]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
