using Application.Features.Messages;
using Domain.ChatAggregate;

namespace Application.Common.Mappings;

public static class MessageMappings
{
    public static MessageDTO ToDto(Message message) => new()
    {
        Id = message.Id,
        Content = message.Content,
        SentDate = message.SentDate,
        SenderId = message.SenderId,
    };
}
