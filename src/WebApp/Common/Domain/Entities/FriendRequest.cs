namespace Domain.Entities;

// dependent entity on AppUser entity
public class FriendRequest
{
    public DateTime Date { get; set; } = DateTime.UtcNow;

    // Foreign Keys (they are both a composite primary key)
    public int RequesterId { get; set; }
    public int RequestedId { get; set; }

    // navigation properties
    public ApplicationUser? Requester { get; set; }
    public ApplicationUser? Requested { get; set; }
}
