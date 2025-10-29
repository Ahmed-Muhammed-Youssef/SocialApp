using Application.Features.Messages;
using Domain.Entities;

namespace Application.Common.Mappings;

public static class MessageMappings
{
    public static MessageDTO ToDto(Message message) => new()
    {
        Id = message.Id,
        Content = message.Content,
        ReadDate = message.ReadDate,
        SentDate = message.SentDate,
        SenderPhotoUrl = message.Sender?.ProfilePictureUrl,
        SenderId = message.SenderId,
        RecipientId = message.RecipientId
    };
}
