using System.ComponentModel.DataAnnotations;

namespace API.Controllers.Account.Requests
{
    public record RegisterRequest
    {
        [Required, MaxLength(255)]
        public string FirstName { get; init; }
        [Required, MaxLength(255)]
        public string LastName { get; init; }
        [Required]
        public char Sex { get; init; }
        [EmailAddress, Required]
        public string Email { get; init; }
        [Required]
        [MinLength(6)]
        public string Password { get; init; }
        [Required]
        public DateTime DateOfBirth { get; init; }
        [Required]
        public int CityId { get; init; }
    }
}
