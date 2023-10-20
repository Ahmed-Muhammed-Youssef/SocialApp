using System.ComponentModel.DataAnnotations;

namespace API.Application.DTOs
{
    public class UpdatedUserDTO
    {

        [Required, MaxLength(255)]
        public string FirstName { get; set; }
        [Required, MaxLength(255)]
        public string LastName { get; set; }
        [Required]
        public char Interest { get; set; }
        public string Bio { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
    }
}
