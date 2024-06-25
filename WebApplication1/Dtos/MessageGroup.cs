namespace WebApplication1.Dtos
{
    public class MessageGroup
    {
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
        public int RecipientId { get; set; }
        public string GroupName { get; set; }
        public List<string> Contents { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
