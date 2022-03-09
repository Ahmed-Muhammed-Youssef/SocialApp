using System;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime? ReadDate { get; set; }
        public DateTime SentDate { get; set; } = DateTime.UtcNow;

        // we only delete the messages from the server only if both of them delted the message
        [Required]
        public bool SenderDeleted { get; set; }
        [Required]
        public bool RecipientDeleted { get; set; }

        [Required]
        public int SenderId { get; set; }
        public AppUser Sender { get; set; }

        [Required]
        public int RecipientId { get; set; }
        public AppUser Recipient { get; set; }
    }
}
