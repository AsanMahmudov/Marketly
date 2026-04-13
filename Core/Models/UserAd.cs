using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketly.Core.Models
{
    public class UserAd
    {
        public string UserId { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        public int AdId { get; set; } 
        [ForeignKey(nameof(AdId))]
        public Ad Ad { get; set; } = null!;
    }
}
