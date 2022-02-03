using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    // dependent entity on AppUser entity
    public class Match 
    {
        // Foreign Key
        [Required]
        public int UserId { get; set; }
        // Reference navigation property
        public AppUser User { get; set; }
        // Foreign Key
        [Required]
        public int MatchedId { get; set; }
        // Reference navigation property
        public AppUser Matched { get; set; }
    }
}
