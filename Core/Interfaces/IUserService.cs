using Marketly.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketly.Core.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileViewModel> GetUserProfileAsync(string userId);
        Task<IEnumerable<AdMinifiedViewModel>> GetFavoriteAdsAsync(string userId);
        Task AddToFavoritesAsync(string userId, int adId);
        Task RemoveFromFavoritesAsync(string userId, int adId);
    }
}
