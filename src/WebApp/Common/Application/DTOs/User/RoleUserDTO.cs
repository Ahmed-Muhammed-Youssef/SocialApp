using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public class RoleUserDTO
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
