namespace Domain.FriendRequestAggregate;

public class FriendRequest: IAggregateRoot
{
    public DateTime Date { get; private set; }
    public int RequesterId { get; private set; }
    public int RequestedId { get; private set; }

    private FriendRequest() { }  // For EF

    // Factory: Send request
    public static FriendRequest Create(int requesterId, int requestedId)
    {
        if (requesterId == requestedId)
            throw new InvalidFriendRequestException("Cannot request self");

        return new FriendRequest
        {
            RequesterId = requesterId,
            RequestedId = requestedId,
            Date = DateTime.UtcNow
        };
    }
}
