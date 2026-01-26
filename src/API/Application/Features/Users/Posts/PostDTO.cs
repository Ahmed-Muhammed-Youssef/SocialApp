namespace Application.Features.Users.Posts;

public record PostDTO
{
    public ulong Id { get; init; }
    public required string Content { get; init; }
    public DateTime DatePosted { get; init; }
    public DateTime? DateEdited { get; init; }
    public int OwnerId { get; init; }
    public string OwnerName { get; init; } = "";
    public string OwnerPictureUrl { get; init; } = "";
}
