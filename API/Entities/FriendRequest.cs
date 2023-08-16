using System;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    // dependent entity on AppUser entity
    public class FriendRequest 
    {
        // Foreign Key
        [Required]
        public int RequesterId { get; set; }
        // Reference navigation property
        public AppUser Requester { get; set; }

        // Foreign Key
        [Required]
        public int RequestedId { get; set; }
        // Reference navigation property
        public AppUser Requested { get; set; }
        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
