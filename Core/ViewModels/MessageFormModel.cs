namespace Marketly.Core.ViewModels
{
    public class MessageFormModel
    {
        public string RecipientId { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int? RelatedAdId { get; set; }
    }
}