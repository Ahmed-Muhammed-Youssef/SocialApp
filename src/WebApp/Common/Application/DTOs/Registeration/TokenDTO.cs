using System.ComponentModel.DataAnnotations;
using Application.DTOs.User;

namespace Application.DTOs.Registeration
{
    public class TokenDTO
    {
        public UserDTO UserData { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
