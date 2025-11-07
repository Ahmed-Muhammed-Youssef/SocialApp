using Application.Features.DirectChats.Stores;

namespace Application.Features.DirectChats.ConnectToChat;

public record ConnectToChatResult(Group Group, IEnumerable<MessageDTO> Messages);