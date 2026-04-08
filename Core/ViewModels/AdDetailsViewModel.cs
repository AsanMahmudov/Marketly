namespace Marketly.Core.ViewModels
{
    public class AdDetailsViewModel
    {
        public int Id { get; set; } 
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public string SellerName { get; set; } = string.Empty;
        public string SellerId { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public IEnumerable<string> ImageUrls { get; set; } = new List<string>();
        public bool IsAvailable { get; set; }
    }
}