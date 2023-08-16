using System;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    // dependent entity on AppUser entity
    public class FriendRequest 
    {
        // Foreign Key
        [Required]
        public int RequisterId { get; set; }
        // Reference navigation property
        public AppUser Requister { get; set; }

        // Foreign Key
        [Required]
        public int RequistedId { get; set; }
        // Reference navigation property
        public AppUser Requisted { get; set; }
        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
