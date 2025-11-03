namespace API.Features.Posts.Requests;

public record CreatePostRequest
{
    [Required]
    public required string Content { get; init; }
}
