namespace API.Domain.Entities
{
    public class Post
    {
        public ulong Id { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime DatePosted { get; set; } = DateTime.UtcNow;
        public DateTime? DateEdited { get; set; } = null;


        // Navigation Properties
        public AppUser AppUser { get; set; }
    }
}
