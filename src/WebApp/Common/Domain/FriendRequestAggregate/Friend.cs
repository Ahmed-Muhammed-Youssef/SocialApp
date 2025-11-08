namespace Domain.FriendRequestAggregate;

public class Friend
{
    public DateTime Created { get; set; } = DateTime.UtcNow;

    // Foreign Keys (composite primary key)
    public int UserId { get; set; }
    public int FriendId { get; set; }
}
