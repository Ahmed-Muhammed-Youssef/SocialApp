using System.ComponentModel.DataAnnotations;
using Application.DTOs.Picture;

namespace Application.Features.Users;

public class UserDTO
{
    [Required]
    public int Id { get; set; }
    [Required, MaxLength(255)]
    public required string FirstName { get; set; }
    [Required, MaxLength(255)]
    public required string LastName { get; set; }
    public string? ProfilePictureUrl { get; set; }
    [Required]
    public char Sex { get; set; }
    [Required]
    public int Age { get; set; }
    [Required]
    public DateTime Created { get; set; }
    [Required]
    public DateTime LastActive { get; set; }
    [Required]
    public required string Bio { get; set; }
    public IEnumerable<PictureDTO> Pictures { get; set; } = [];
}
