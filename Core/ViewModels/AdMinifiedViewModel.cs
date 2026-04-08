namespace Marketly.Core.ViewModels
{
    public class AdMinifiedViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string SellerName { get; set; } = string.Empty;
    }
}