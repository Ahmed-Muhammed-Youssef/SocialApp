namespace Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime? ReadDate { get; set; }
        public DateTime SentDate { get; set; } = DateTime.UtcNow;

        // we only delete the messages from the server only if both of them deleted the message
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }

        // foreign keys
        public int SenderId { get; set; }
        public int RecipientId { get; set; }

        //navigation preoperties
        public AppUser Sender { get; set; }
        public AppUser Recipient { get; set; }
    }
}
