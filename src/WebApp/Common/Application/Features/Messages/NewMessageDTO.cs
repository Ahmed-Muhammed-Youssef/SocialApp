using System.ComponentModel.DataAnnotations;

namespace Application.Features.Messages;

public record NewMessageDTO
{
    [Required]
    public int RecipientId { get; init; }
    [Required, MinLength(1), MaxLength(300)]
    public required string Content { get; init; }
}
