namespace Application.Features.DirectChats;

public record MessageDTO
{
    public int Id { get; init; }
    [Required]
    public required string Content { get; init; }
    public DateTime SentDate { get; init; }
    [Required]
    public int SenderId { get; init; }

    [Required]
    public int RecipientId { get; init; }
}
