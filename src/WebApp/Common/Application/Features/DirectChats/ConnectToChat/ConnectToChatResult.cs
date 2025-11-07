using Application.Features.DirectChats;

namespace Application.Features.DirectChats.ConnectToChat;

public record ConnectToChatResult(Group Group, IEnumerable<MessageDTO> Messages);