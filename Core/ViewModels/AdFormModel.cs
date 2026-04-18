using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Marketly.Core.ViewModels
{
    public class AdFormModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public IEnumerable<AdCategoryViewModel> Categories { get; set; } = new List<AdCategoryViewModel>();
        public IEnumerable<IFormFile> ImageFiles { get; set; } = new List<IFormFile>();
    }
}