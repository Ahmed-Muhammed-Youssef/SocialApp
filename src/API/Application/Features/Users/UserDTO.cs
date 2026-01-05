namespace Application.Features.Users;

public record UserDTO
{
    [Required]
    public int Id { get; init; }
    [Required, MaxLength(255)]
    public required string FirstName { get; init; }
    [Required, MaxLength(255)]
    public required string LastName { get; init; }
    [Required]
    public Gender Gender { get; init; }
    [Required]
    public int Age { get; init; }
    [Required]
    public DateTime Created { get; init; }
    [Required]
    public DateTime LastActive { get; init; }
    [Required]
    public required string Bio { get; init; }
    public RelationStatus RelationStatus { get; init; } = RelationStatus.None;
    public required string? ProfilePictureUrl { get; init; } = null;
    public IEnumerable<PictureDTO> Pictures { get; init; } = [];
}
