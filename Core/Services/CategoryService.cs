using Marketly.Core.Common;
using Marketly.Core.Interfaces;
using Marketly.Core.Models;
using Marketly.Core.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Marketly.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository repository;
        public CategoryService(IRepository _repository) => repository = _repository;

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

        public async Task CreateAsync(string name)
        {
            await repository.AddAsync(new Category { Name = name });
            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await repository.All<Category>().FirstOrDefaultAsync(c => c.Id == id);
            if (category != null)
            {
                repository.Delete(category);
                await repository.SaveChangesAsync();
            }
        }
    }
}