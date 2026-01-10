namespace Application.Features.DirectChats;

public record MessageDTO
{
    public int Id { get; init; }
    public required string Content { get; init; }
    public DateTime SentDate { get; init; }
    public int SenderId { get; init; }
    public int RecipientId { get; init; }
}
