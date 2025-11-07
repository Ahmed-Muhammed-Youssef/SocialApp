using Application.Features.DirectChats;

namespace Application.Features.DirectChats.SendMessage;

public record SendMessageResult(MessageDTO MessageDTO, string GroupName);
