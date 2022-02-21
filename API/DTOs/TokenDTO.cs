using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class TokenDTO
    {
        public UserDTO UserData { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
