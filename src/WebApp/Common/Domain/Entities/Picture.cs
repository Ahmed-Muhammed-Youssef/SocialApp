namespace Domain.Entities
{
    public class Picture
    {
        public int Id { get; set; }
        public required string Url { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public required string PublicId { get; set; }

        // foreing key
        public int AppUserId { get; set; }

        // navigation properties
        public ApplicationUser? AppUser { get; set; }

    }
}