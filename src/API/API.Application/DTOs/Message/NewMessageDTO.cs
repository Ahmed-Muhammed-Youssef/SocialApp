using System.ComponentModel.DataAnnotations;

namespace API.Application.DTOs.Message
{
    public class NewMessageDTO
    {
        [Required]
        public int RecipientId { get; set; }
        [Required, MinLength(1), MaxLength(300)]
        public string Content { get; set; }
    }
}
