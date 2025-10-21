using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Post;

public class CreatePostDTO
{
    [Required]
    public required string Content { get; set; }
}
