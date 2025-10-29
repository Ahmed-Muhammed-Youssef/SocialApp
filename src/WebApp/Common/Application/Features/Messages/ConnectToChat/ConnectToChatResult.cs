using Domain.Entities;

namespace Application.Features.Messages.ConnectToChat;

public record ConnectToChatResult(Group Group, IEnumerable<MessageDTO> Messages);