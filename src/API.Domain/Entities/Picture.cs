namespace API.Domain.Entities
{
    public class Picture
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public string PublicId { get; set; }

        // foreing key
        public int AppUserId { get; set; }

        // navigation properties
        public AppUser AppUser { get; set; }

    }
}