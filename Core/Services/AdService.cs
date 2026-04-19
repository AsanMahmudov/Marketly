using Marketly.Core.Common;
using Marketly.Core.Interfaces;
using Marketly.Core.Models;
using Marketly.Core.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Marketly.Core.Services
{
    public class AdService : IAdService
    {
        private readonly IRepository repository;
        private readonly IWebHostEnvironment environment;

        public AdService(IRepository _repository, IWebHostEnvironment _environment)
        {
            repository = _repository;
            environment = _environment;
        }

        public async Task<AdQueryModel> AllAsync(string? category = null, string? searchTerm = null, int currentPage = 1, int adsPerPage = 1)
        {
            var adsQuery = repository.All<Ad>().Where(a => a.IsActive);

            if (!string.IsNullOrWhiteSpace(category))
            {
                adsQuery = adsQuery.Where(a => a.Category.Name == category);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string normalizedSearch = searchTerm.ToLower();
                adsQuery = adsQuery.Where(a => a.Title.ToLower().Contains(normalizedSearch) ||
                                               a.Description.ToLower().Contains(normalizedSearch));
            }

            var ads = await adsQuery
                .Skip((currentPage - 1) * adsPerPage)
                .Take(adsPerPage)
                .Select(a => new AdMinifiedViewModel
                {
                    Id = a.Id,
                    Title = a.Title,
                    Price = a.Price,
                    Category = a.Category.Name,
                    ImageUrl = a.Images.Any() ? a.Images.First().Url : "/img/no-image.png",
                    SellerName = a.Seller.UserName!
                }).ToListAsync();

            int totalAds = await adsQuery.CountAsync();

            return new AdQueryModel
            {
                TotalAds = totalAds,
                Ads = ads,
                TotalPages = (int)Math.Ceiling((double)totalAds / adsPerPage),
                CurrentPage = currentPage
            };
        }

        public async Task<AdDetailsViewModel> GetDetailsByIdAsync(int id)
        {
            return await repository.All<Ad>()
                .Where(a => a.Id == id)
                .Select(a => new AdDetailsViewModel
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    Price = a.Price,
                    Category = a.Category.Name,
                    SellerName = a.Seller.UserName!,
                    SellerId = a.SellerId,
                    CreatedDate = a.CreatedOn,
                    ImageUrls = a.Images.Select(i => i.Url),
                    IsAvailable = a.IsActive
                }).FirstAsync();
        }

        public async Task<int> CreateAsync(AdFormModel model, string sellerId)
        {
            var ad = new Ad
            {
                Title = model.Title,
                Description = model.Description,
                Price = model.Price,
                CategoryId = model.CategoryId,
                SellerId = sellerId,
                CreatedOn = DateTime.UtcNow,
                IsActive = true
            };

            // Image processing logic
            if (model.ImageFiles != null && model.ImageFiles.Any())
            {
                string uploadDir = Path.Combine(environment.WebRootPath, "img", "ads");
                if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

                foreach (var file in model.ImageFiles)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filePath = Path.Combine(uploadDir, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    ad.Images.Add(new Image { Url = "/img/ads/" + fileName });
                }
            }

            await repository.AddAsync(ad);
            await repository.SaveChangesAsync();
            return ad.Id;
        }

        public async Task EditAsync(int id, AdFormModel model)
        {
            var ad = await repository.All<Ad>().FirstAsync(a => a.Id == id);
            ad.Title = model.Title;
            ad.Description = model.Description;
            ad.Price = model.Price;
            ad.CategoryId = model.CategoryId;

            await repository.SaveChangesAsync();
        }

        public async Task<AdFormModel> GetFormModelByIdAsync(int id)
        {
            return await repository.All<Ad>()
                .Where(a => a.Id == id)
                .Select(a => new AdFormModel
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    Price = a.Price,
                    CategoryId = a.CategoryId
                }).FirstAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ad = await repository.All<Ad>().FirstAsync(a => a.Id == id);
            ad.IsActive = false; // Soft delete is safer for marketplaces
            await repository.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id) => await repository.All<Ad>().AnyAsync(a => a.Id == id);

        public async Task<bool> IsSellerWithIdAsync(int adId, string userId) =>
            await repository.All<Ad>().AnyAsync(a => a.Id == adId && a.SellerId == userId);

        public async Task<IEnumerable<AdMinifiedViewModel>> GetLatestAsync(int count)
        {
            return await repository.All<Ad>()
                .Where(a => a.IsActive)
                .OrderByDescending(a => a.CreatedOn) // Gets newest first
                .Take(count)
                .Select(a => new AdMinifiedViewModel
                {
                    Id = a.Id,
                    Title = a.Title,
                    Price = a.Price,
                    Category = a.Category.Name,
                    // Uses the first image if it exists, otherwise a placeholder
                    ImageUrl = a.Images.Any() ? a.Images.First().Url : "/img/no-image.png"
                })
                .ToListAsync();
        }
    }
}