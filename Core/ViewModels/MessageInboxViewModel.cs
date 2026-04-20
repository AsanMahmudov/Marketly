    namespace Marketly.Core.ViewModels
{
    public class MessageInboxViewModel
    {
        public int Id { get; set; }
        public string SenderId { get; set; } = null!;
        public string ReceiverId { get; set; } = null!; 
        public string SenderName { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime SentDate { get; set; }
        public int RelatedAdId { get; set; }
        public bool IsRead { get; set; }
    }
}