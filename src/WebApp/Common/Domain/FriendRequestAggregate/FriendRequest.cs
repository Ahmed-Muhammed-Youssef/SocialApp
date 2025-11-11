namespace Domain.FriendRequestAggregate;

public class FriendRequest: EntityBase, IAggregateRoot
{
    public DateTime Date { get; private set; }
    public int RequesterId { get; private set; }
    public int RequestedId { get; private set; }

    public RequestStatus Status { get; private set; }

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
            Status = RequestStatus.Pending,
            Date = DateTime.UtcNow
        };
    }

    public void Accept(int accepterId)  // Called in app handler
    {
        if (Status != RequestStatus.Pending || accepterId != RequestedId)
            throw new InvalidFriendRequestException("Only pending requests can be accepted by recipient");
        Status = RequestStatus.Accepted;
    }
}
