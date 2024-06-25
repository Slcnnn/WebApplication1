using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class GroupChat
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SenderId { get; set; }
        public virtual AppUser Sender { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
    }
}
