namespace Domain.Entities
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public required string IdentityId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? ProfilePictureUrl { get; set; } = null;
        public char Sex { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public string? Bio { get; set; }
        public int CityId { get; set; }

        // navigation properties
        public ICollection<FriendRequest> FriendRequestsSent { get; set; } = [];
        public ICollection<FriendRequest> FriendRequestsReceived { get; set; } = [];
        public ICollection<Message> MessagesSent { get; set; } = [];
        public ICollection<Message> MessagesReceived { get; set; } = [];
        public ICollection<Friend> Friends { get; set; } = [];
        public ICollection<Picture> Pictures { get; set; } = [];
        public ICollection<Post> Posts { get; set; } = [];
    }
}
