using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketly.Core.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Url { get; set; } = null!;

        public int AdId { get; set; }
        [ForeignKey(nameof(AdId))]
        public Ad Ad { get; set; } = null!;
    }
}
