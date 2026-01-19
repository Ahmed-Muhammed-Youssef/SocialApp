namespace Domain.ApplicationUserAggregate;

public class Post
{
    public ulong Id { get; set; }
    public int UserId { get; set; }
    public required string Content { get; set; }
    public DateTime DatePosted { get; set; } = DateTime.UtcNow;
    public DateTime? DateEdited { get; set; }


    // Navigation Properties
    public ApplicationUser? ApplicationUser { get; set; }
}
