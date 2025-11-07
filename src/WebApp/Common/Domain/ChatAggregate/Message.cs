namespace Domain.ChatAggregate;

public class Message : EntityBase
{
    private Message() { }
    public Message(int senderId, string content)
    {
        SenderId = senderId;
        Content = content;
    }
    public string Content { get; private set; } = string.Empty;
    public DateTime SentDate { get; private set; } = DateTime.UtcNow;
    public int SenderId { get; private set; }
}
