namespace Domain.DirectChatAggregate;

public class DirectChat : EntityBase, IAggregateRoot
{
    private readonly List<Message> _messages = [];
    private DirectChat() { }
    public DirectChat(int user1Id, int user2Id)
    {
        (User1Id, User2Id) = OrderIds(user1Id, user2Id);
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
    /// <exception cref="InvalidMessageException">Thrown if <paramref name="content"/> is null or empty.</exception>
    public Message AddMessage(int senderId, string content)
    {
        if (!Involves(senderId))
        {
            throw new InvalidSenderException("Sender is not a participant of the chat.");
        }

        if(string.IsNullOrWhiteSpace(content))
        {
            throw new InvalidMessageException("Message content cannot be null or empty.");
        }
        var newMessage = new Message(Id, senderId, content);

        _messages.Add(newMessage);
        
        return newMessage;
    }

    private bool Involves(int userId) => userId == User1Id || userId == User2Id;

    /// <summary>
    /// Returns a tuple containing the IDs of two users, ordered in ascending order.
    /// </summary>
    /// <param name="user1Id">The ID of the first user.</param>
    /// <param name="user2Id">The ID of the second user.</param>
    /// <returns>A tuple with two elements: <see langword="User1Id"/> and <see langword="User2Id"/>, where <see
    /// langword="User1Id"/> is the smaller of the two input IDs, and <see langword="User2Id"/> is the larger.</returns>
    public static (int User1Id, int User2Id) OrderIds(int user1Id, int user2Id) => user1Id < user2Id ? (user1Id, user2Id) : (user2Id, user1Id);
}
