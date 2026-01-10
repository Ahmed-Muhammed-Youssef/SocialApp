namespace API.Features.Users.Requests;

public class UpdateUserRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? Bio { get; set; }
    public required int CityId { get; set; }
}
