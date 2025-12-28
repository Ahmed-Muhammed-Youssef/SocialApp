namespace Domain.FriendAggregate;

public class Friend : IAggregateRoot
{
    public DateTime Created { get; private set; }
    public int UserId { get; private set; }
    public int FriendId { get; private set; }

    public static Friend CreateFromAcceptedRequest(int user1Id, int user2Id)
    {
        if (user1Id == user2Id)
        {
            throw new InvalidFriendParametersException("you cannot befriend yourself!");
        }

        (user1Id, user2Id) = OrderIds(user1Id, user2Id);

        return new Friend
        {
            UserId = user1Id,
            FriendId = user2Id,
            Created = DateTime.UtcNow,
        };
    }

    private static (int, int) OrderIds(int idA, int idB) => idA < idB ? (idA, idB) : (idB, idA);
}
