using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User;

public class RoleUserDTO
{
    [EmailAddress]
    public required string Email { get; set; }
    public required string Role { get; set; }
}
