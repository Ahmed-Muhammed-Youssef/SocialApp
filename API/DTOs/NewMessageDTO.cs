using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class NewMessageDTO
    {
        [Required]
        public string RecipientUsername { get; set; }
        [Required, MinLength(1), MaxLength(300)]
        public string Content { get; set; }
    }
}
