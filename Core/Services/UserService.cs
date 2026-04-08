using Marketly.Core.Interfaces;
using Marketly.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketly.Core.Services
{
    public class UserService : IUserService
    {
        public Task AddToFavoritesAsync(string userId, int adId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AdMinifiedViewModel>> GetFavoriteAdsAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<UserProfileViewModel> GetUserProfileAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromFavoritesAsync(string userId, int adId)
        {
            throw new NotImplementedException();
        }
    }
}
