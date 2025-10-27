using Application.Features.Messages;

namespace MVC.Models;

public class ChatViewModel
{
    public int Id { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public IEnumerable<MessageDTO> MessagesThread { get; set; } = [];
}
