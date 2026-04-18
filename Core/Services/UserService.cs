using Marketly.Core.Common;
using Marketly.Core.Interfaces;
using Marketly.Core.Models;
using Marketly.Core.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Marketly.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository repository;
        public UserService(IRepository _repository) => repository = _repository;

        public async Task<UserProfileViewModel> GetUserProfileAsync(string userId)
        {
            return await repository.All<ApplicationUser>()
                .Where(u => u.Id == userId)
                .Select(u => new UserProfileViewModel
                {
                    Id = u.Id,
                    Username = u.UserName!,
                    Email = u.Email!,
                    UserAds = u.OwnedAds.Select(a => new AdMinifiedViewModel
                    {
                        Id = a.Id,
                        Title = a.Title,
                        Price = a.Price,
                        ImageUrl = a.Images.Any() ? a.Images.First().Url : "/img/no-image.png"
                    })
                }).FirstAsync();
        }

        public async Task<IEnumerable<AdMinifiedViewModel>> GetFavoriteAdsAsync(string userId)
        {
            return await repository.All<UserAd>()
                .Where(ua => ua.UserId == userId)
                .Select(ua => new AdMinifiedViewModel
                {
                    Id = ua.Ad.Id,
                    Title = ua.Ad.Title,
                    Price = ua.Ad.Price,
                    ImageUrl = ua.Ad.Images.Any() ? ua.Ad.Images.First().Url : "/img/no-image.png"
                }).ToListAsync();
        }

        public async Task AddToFavoritesAsync(string userId, int adId)
        {
            if (!await repository.All<UserAd>().AnyAsync(ua => ua.UserId == userId && ua.AdId == adId))
            {
                await repository.AddAsync(new UserAd { UserId = userId, AdId = adId });
                await repository.SaveChangesAsync();
            }
        }

        public async Task RemoveFromFavoritesAsync(string userId, int adId)
        {
            var favorite = await repository.All<UserAd>()
                .FirstOrDefaultAsync(ua => ua.UserId == userId && ua.AdId == adId);

            if (favorite != null)
            {
                repository.Delete(favorite);
                await repository.SaveChangesAsync();
            }
        }
    }
}