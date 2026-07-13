namespace API.Features.Posts.Requests;

/// <summary>
/// Request to create a new post.
/// </summary>
public record CreatePostRequest
{
    /// <summary>The text content of the post.</summary>
    public required string Content { get; init; }
}
