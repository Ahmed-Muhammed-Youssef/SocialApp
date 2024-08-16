namespace Domain.Entities
{
    // dependent entity on AppUser entity
    public class Friend
    {
        public DateTime Created { get; set; } = DateTime.UtcNow;

        // Foreign Keys (composite primary key)
        public int UserId { get; set; }
        public int FriendId { get; set; }
        // navigation properties
        public AppUser User { get; set; }
        public AppUser FriendUser { get; set; }
    }
}
