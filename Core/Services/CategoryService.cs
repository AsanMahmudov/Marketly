using Marketly.Core.Common; // For IApplicationRepository
using Marketly.Core.Interfaces;
using Marketly.Core.Models;
using Marketly.Core.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Marketly.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IApplicationRepository repository;

        public CategoryService(IApplicationRepository _repository)
            => repository = _repository;

        public async Task<IEnumerable<AdCategoryViewModel>> AllCategoriesAsync()
        {
            return await repository.All<Category>()
                .OrderBy(c => c.Name)
                .Select(c => new AdCategoryViewModel { Id = c.Id, Name = c.Name })
                .ToListAsync();
        }

        public async Task<IEnumerable<CategoryAdminViewModel>> GetAllForAdminAsync()
        {
            return await repository.All<Category>()
                .Select(c => new CategoryAdminViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    AdCount = c.Ads.Count()
                }).ToListAsync();
        }

        public async Task CreateAsync(CategoryFormModel model)
        {
            await repository.AddAsync(new Category { Name = model.Name });
            await repository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await repository.All<Category>()
                .Include(c => c.Ads)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null || category.Ads.Any())
            {
                return false; // Safety check: block if ads exist
            }

            repository.Delete(category);
            await repository.SaveChangesAsync();
            return true;
        }
    }
}