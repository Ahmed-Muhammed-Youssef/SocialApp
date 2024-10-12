using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Registeration
{
    public class RegisterDTO
    {
        [Required, MaxLength(255)]
        public string FirstName { get; set; }
        [Required, MaxLength(255)]
        public string LastName { get; set; }
        [Required]
        public char Sex { get; set; }
        [EmailAddress, Required]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public int CityId { get; set; }
    }
}
