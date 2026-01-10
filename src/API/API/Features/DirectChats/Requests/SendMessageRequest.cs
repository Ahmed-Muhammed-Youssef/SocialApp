namespace API.Features.DirectChats.Requests;

public record SendMessageRequest
{
    public int RecipientId { get; init; }
    public required string Content { get; init; }
}
