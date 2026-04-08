using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketly.Core.ViewModels
{
    public class UserProfileViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? City { get; set; }
        public DateTime JoinDate { get; set; }
        public IEnumerable<AdMinifiedViewModel> UserAds { get; set; } = new List<AdMinifiedViewModel>();
    }
}
