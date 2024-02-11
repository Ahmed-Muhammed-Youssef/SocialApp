namespace API.Application.DTOs.Post
{
    public class PostDTO
    {
        public ulong Id { get; set; }
        public string Content { get; set; }
        public DateTime DatePosted { get; set; }
        public DateTime? DateEdited { get; set; } = null;
    }
}
