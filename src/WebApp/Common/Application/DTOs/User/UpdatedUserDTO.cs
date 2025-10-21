using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User;

public class UpdatedUserDTO
{

    [Required, MaxLength(255)]
    public required string FirstName { get; set; }
    [Required, MaxLength(255)]
    public required string LastName { get; set; }
    [Required]
    public char Interest { get; set; }
    public string? Bio { get; set; }
    [Required]
    public required string City { get; set; }
    [Required]
    public required string Country { get; set; }
}
