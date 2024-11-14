namespace Application.DTOs.Message
{
    public class ChatDTO
    {
        public int Id { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<MessageDTO> MessagesThread { get; set; }
    }
}
