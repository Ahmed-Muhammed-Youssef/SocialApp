using System.ComponentModel.DataAnnotations;

namespace API.Controllers.Account.Requests;

public record LoginRequest
{
    [Required, EmailAddress]
    public string Email { get; init; }
    [Required]
    public string Password { get; init; }
}
