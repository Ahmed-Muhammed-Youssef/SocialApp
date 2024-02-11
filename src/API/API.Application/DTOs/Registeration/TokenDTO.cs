using System.ComponentModel.DataAnnotations;
using API.Application.DTOs.User;

namespace API.Application.DTOs.Registeration
{
    public class TokenDTO
    {
        public UserDTO UserData { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
