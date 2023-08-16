using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    // dependent entity on AppUser entity
    public class Friend 
    {
        // Foreign Key
        [Required]
        public int UserId { get; set; }
        // Reference navigation property
        public AppUser User { get; set; }
        // Foreign Key
        [Required]
        public int FriendId { get; set; }
        // Reference navigation property
        public AppUser FriendUser { get; set; }
    }
}
