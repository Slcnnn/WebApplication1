using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SenderId { get; set; }
        public AppUser Sender { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int RecipientId { get; set; }
        public AppUser Recipient { get; set; }
    }
}
