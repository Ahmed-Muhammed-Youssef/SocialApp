using System.ComponentModel.DataAnnotations;

namespace Application.Features.Messages;

public record MessageDTO
{
    public int Id { get; init; }
    [Required]
    public required string Content { get; init; }
    public DateTime? ReadDate { get; init; }
    [Required]
    public DateTime SentDate { get; init; }

    [Required]
    public string? SenderPhotoUrl { get; init; }

    [Required]
    public int SenderId { get; init; }

    [Required]
    public int RecipientId { get; init; }
}
