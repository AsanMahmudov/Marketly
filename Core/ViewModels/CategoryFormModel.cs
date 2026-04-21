using System.ComponentModel.DataAnnotations;
using static Marketly.Core.Common.DataValidationConstants.Category;

namespace Marketly.Core.ViewModels
{
    public class CategoryFormModel
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength,
            ErrorMessage = "Category name must be between {2} and {1} characters.")]
        [Display(Name = "Category Name")]
        public string Name { get; set; } = string.Empty;
    }
}