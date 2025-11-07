namespace Domain.ChatAggregate;

public class DirectChat : EntityBase, IAggregateRoot
{
    private readonly List<Message> _messages = [];
    private DirectChat() { }
    public DirectChat(int user1Id, int user2Id)
    {
        (User1Id, User2Id) = user1Id < user2Id ? (user1Id, user2Id) : (user2Id, user1Id);
    }
    public int User1Id { get; private set; }
    public int User2Id { get; private set; }
    public IReadOnlyCollection<Message> Messages => _messages.AsReadOnly();

    /// <summary>
    /// Adds a new message to the chat from the specified sender.
    /// </summary>
    /// <param name="senderId">The unique identifier of the sender. Must be a participant of the chat.</param>
    /// <param name="content">The content of the message to be added. Cannot be null or empty.</param>
    /// <exception cref="InvalidSenderException">Thrown if <paramref name="senderId"/> is not a participant of the chat.</exception>
    public void AddMessage(int senderId, string content)
    {
        if (!Involves(senderId))
        {
            throw new InvalidSenderException("Sender is not a participant of the chat.");
        }

        if(string.IsNullOrEmpty(content))
        {
            throw new InvalidMessageException("Message content cannot be null or empty.");
        }

        _messages.Add(new Message(Id, senderId, content));
    }

    private bool Involves(int userId) => userId == User1Id || userId == User2Id;

}
