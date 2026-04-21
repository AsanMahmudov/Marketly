using System.ComponentModel.DataAnnotations;
using static Marketly.Core.Common.DataValidationConstants.Category;

namespace Marketly.Core.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public ICollection<Ad> Ads { get; set; } = new HashSet<Ad>();
    }
}