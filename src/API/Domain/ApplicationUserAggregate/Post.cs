namespace Domain.ApplicationUserAggregate;

public class Post : EntityBase<ulong>
{
    public int UserId { get; private set; }
    public string Content { get; private set; } = string.Empty;
    public DateTime DatePosted { get; private set; }
    public DateTime? DateEdited { get; private set; }

    // Navigation Properties
    public ApplicationUser? ApplicationUser { get; private set; }

    private Post() { } // For EF Core

    // Factory method
    public static Post Create(int userId, string content)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("User ID must be valid.", nameof(userId));
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Post content cannot be empty.", nameof(content));
        }

        if (content.Length > SystemPolicy.PostMaxContentLength)
        {
            throw new ArgumentException($"Post content cannot exceed {SystemPolicy.PostMaxContentLength} characters.", nameof(content));
        }

        return new Post
        {
            UserId = userId,
            Content = content.Trim(),
            DatePosted = DateTime.UtcNow
        };
    }

    // Behavior method
    public void Edit(string newContent)
    {
        if (string.IsNullOrWhiteSpace(newContent))
        {
            throw new ArgumentException("Post content cannot be empty.", nameof(newContent));
        }

        if (newContent.Length > 5000)
        {
            throw new ArgumentException("Post content cannot exceed 5000 characters.", nameof(newContent));
        }

        Content = newContent.Trim();
        DateEdited = DateTime.UtcNow;
    }
}
