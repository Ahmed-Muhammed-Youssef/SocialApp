using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Message;

public class NewMessageDTO
{
    [Required]
    public int RecipientId { get; set; }
    [Required, MinLength(1), MaxLength(300)]
    public required string Content { get; set; }
}
