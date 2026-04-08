    namespace Marketly.Core.ViewModels
{
    public class MessageInboxViewModel
    {
        public int Id { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string SenderId { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime SentDate { get; set; }
        public bool IsRead { get; set; }
        public int? RelatedAdId { get; set; }
    }
}