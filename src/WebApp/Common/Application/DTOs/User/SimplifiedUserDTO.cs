namespace Application.DTOs.User;

public class SimplifiedUserDTO
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? ProfilePictureUrl { get; set; }
}
