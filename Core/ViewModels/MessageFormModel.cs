using System.ComponentModel.DataAnnotations;
using static Marketly.Core.Common.DataValidationConstants.Message;

namespace Marketly.Core.ViewModels
{
    public class MessageFormModel
    {
        [Required]
        public string RecipientId { get; set; } = string.Empty;

        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength,
            ErrorMessage = "Message must be between {2} and {1} characters.")]
        public string Content { get; set; } = string.Empty;

        public int? RelatedAdId { get; set; }
    }
}