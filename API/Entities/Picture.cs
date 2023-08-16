using System;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Picture
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public DateTime Created { get; set; } = DateTime.UtcNow;
        [Required]
        public string PublicId { get; set; }
        public int AppUserId { get; set; }

        public AppUser AppUser { get; set; }

    }
}