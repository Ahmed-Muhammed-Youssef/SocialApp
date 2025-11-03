namespace API.Features.Messages.Requests;

public record SendMessageRequest
{
    [Required]
    public int RecipientId { get; init; }
    [Required, MinLength(1), MaxLength(300)]
    public required string Content { get; init; }
}
