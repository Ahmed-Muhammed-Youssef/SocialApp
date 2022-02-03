using API.Entities;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class UserDTO
    {
        [Required, MaxLength(255)]
        public string FirstName { get; set; }
        [Required, MaxLength(255)]
        public string LastName { get; set; }
        [Required]
        public Sex Sex { get; set; }
        [Required]
        public Interest Interest { get; set; }
    }
}
