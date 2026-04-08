namespace Marketly.Core.ViewModels
{
    public class AdQueryModel
    {
        public IEnumerable<AdMinifiedViewModel> Ads { get; set; } = new List<AdMinifiedViewModel>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalAds { get; set; }
        public string? SelectedCategory { get; set; }
        public string? SearchTerm { get; set; }
        public IEnumerable<AdCategoryViewModel> Categories { get; set; } = new List<AdCategoryViewModel>();
    }
}