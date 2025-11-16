namespace Domain.DirectChatAggregate;

public class Message : EntityBase
{
    private Message() { }
    public Message(int chatId, int senderId, string content)
    {
        ChatId = chatId;
        SenderId = senderId;
        Content = content;
    }
    public int ChatId { get; set; }
    public string Content { get; private set; } = string.Empty;
    public DateTime SentDate { get; private set; } = DateTime.UtcNow;
    public int SenderId { get; private set; }
}
