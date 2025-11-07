namespace Application.Features.DirectChats;

public record DirectChatDTO
{
    public required PagedList<MessageDTO> Messages { get; init; }
}
