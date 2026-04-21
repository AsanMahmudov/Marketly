using System.ComponentModel.DataAnnotations;

namespace Marketly.Core.ViewModels
{
    public class CategoryFormModel
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Category name must be between 3 and 50 characters.")]
        [Display(Name = "Category Name")]
        public string Name { get; set; } = string.Empty;
    }
}