namespace Application.DTOs.Post;

public class PostDTO
{
    public ulong Id { get; set; }
    public required string Content { get; set; }
    public DateTime DatePosted { get; set; }
    public DateTime? DateEdited { get; set; } = null;
    public int OwnerId { get; set; }
    public string OwnerName { get; set; } = "";
    public string OwnerPictureUrl { get; set; } = "";
}
