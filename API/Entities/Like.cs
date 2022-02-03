using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    // dependent entity on AppUser entity
    public class Like 
    {
        // Foreign Key
        [Required]
        public int LikerId { get; set; }
        // Reference navigation property
        public AppUser Liker { get; set; }

        // Foreign Key
        [Required]
        public int LikedId { get; set; }
        // Reference navigation property
        public AppUser Liked { get; set; }
        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
