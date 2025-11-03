namespace API.Features.Users.Requests;

public class UpdateUserRequest
{
    [Required, MaxLength(255)]
    public required string FirstName { get; set; }
    [Required, MaxLength(255)]
    public required string LastName { get; set; }
    public string? Bio { get; set; }
    [Required]
    public required int CityId { get; set; }
}
