using Marketly.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketly.Core.Interfaces
{
    public interface IAdService
    {
        Task<AdQueryModel> AllAsync(string? category = null, string? searchTerm = null, int currentPage = 1, int adsPerPage = 1);

        Task<AdDetailsViewModel> GetDetailsByIdAsync(int id);
        Task<int> CreateAsync(AdFormModel model, string sellerId);
        Task EditAsync(int id, AdFormModel model);
        Task<AdFormModel> GetFormModelByIdAsync(int id);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> IsSellerWithIdAsync(int adId, string userId);
    }
}
