using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketly.Core.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; } 

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; } = null!;

        public DateTime SentOn { get; set; } = DateTime.UtcNow;

        public string SenderId { get; set; } = null!;
        public string ReceiverId { get; set; } = null!;

        public int AdId { get; set; } 
        [ForeignKey(nameof(AdId))]
        public Ad Ad { get; set; } = null!;
    }
}
