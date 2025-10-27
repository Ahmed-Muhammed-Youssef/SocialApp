using System.ComponentModel.DataAnnotations;

namespace API.Controllers.Posts.Requests;

public record CreatePostRequest
{
    [Required]
    public required string Content { get; init; }
}
