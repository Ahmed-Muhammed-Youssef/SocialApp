namespace API.Features.Posts.Requests;

public record CreatePostRequest
{
    public required string Content { get; init; }
}
