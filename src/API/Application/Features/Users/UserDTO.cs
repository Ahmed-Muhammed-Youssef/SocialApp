namespace Application.Features.Users;

public record UserDTO
{
    public int Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public Gender Gender { get; init; }
    public int Age { get; init; }
    public DateTime Created { get; init; }
    public DateTime LastActive { get; init; }
    public required string Bio { get; init; }
    public RelationStatus RelationStatus { get; init; } = RelationStatus.None;
    public required string? ProfilePictureUrl { get; init; }
    public IEnumerable<PictureDTO> Pictures { get; init; } = [];
}
