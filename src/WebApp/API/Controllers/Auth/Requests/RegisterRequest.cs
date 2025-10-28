using System.ComponentModel.DataAnnotations;

namespace API.Controllers.Auth.Requests;

public record RegisterRequest
{
    [Required, MaxLength(255)]
    public required string FirstName { get; init; }
    [Required, MaxLength(255)]
    public required string LastName { get; init; }
    [Required]
    public char Sex { get; init; }
    [EmailAddress, Required]
    public required string Email { get; init; }
    [Required]
    [MinLength(6)]
    public required string Password { get; init; }
    [Required]
    public DateTime DateOfBirth { get; init; }
    [Required]
    public int CityId { get; init; }
}
