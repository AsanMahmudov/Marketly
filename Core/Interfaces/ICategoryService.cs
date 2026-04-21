using Marketly.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketly.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<AdCategoryViewModel>> AllCategoriesAsync();
        Task<IEnumerable<CategoryAdminViewModel>> GetAllForAdminAsync();
        Task CreateAsync(CategoryFormModel model);
        Task<bool> DeleteAsync(int id);           
    }
}
