using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class PhotoReceivedDTO
    {
        [Required]
        public string Url { get; set; }
        [Required]
        public int AppUserId { get; set; }
    }
}
