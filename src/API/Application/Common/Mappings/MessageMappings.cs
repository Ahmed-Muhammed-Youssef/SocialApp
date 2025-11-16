namespace Application.Common.Mappings;

public static class MessageMappings
{
    public static MessageDTO ToDto(Message message) => new()
    {
        Id = message.Id,
        Content = message.Content,
        SentDate = message.SentDate,
        SenderId = message.SenderId
    };

    public static Expression<Func<Message, MessageDTO>> ToDtoExpression = message => new MessageDTO
    {
        Id = message.Id,
        Content = message.Content,
        SentDate = message.SentDate,
        SenderId = message.SenderId
    };
}
