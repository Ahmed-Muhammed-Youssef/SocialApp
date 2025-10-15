using System.ComponentModel.DataAnnotations;

namespace API.Controllers.Account.Responses;

public record AuthResponse
{
    public UserDTO UserData { get; init; }
    [Required]
    public string Token { get; init; }
}
