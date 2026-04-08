using Marketly.Core.Interfaces;
using Marketly.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketly.Core.Services
{
    public class CategoryService : ICategoryService
    {
        public Task<IEnumerable<AdCategoryViewModel>> AllCategoriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CategoryAdminViewModel>> GetAllForAdminAsync()
        {
            throw new NotImplementedException();
        }
    }
}
