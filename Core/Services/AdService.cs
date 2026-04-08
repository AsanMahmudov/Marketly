using Marketly.Core.Interfaces;
using Marketly.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketly.Core.Services
{
    public class AdService : IAdService
    {
        public Task<AdQueryModel> AllAsync(string? category = null, string? searchTerm = null, int currentPage = 1, int adsPerPage = 1)
        {
            throw new NotImplementedException();
        }

        public Task<int> CreateAsync(AdFormModel model, string sellerId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task EditAsync(int id, AdFormModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AdDetailsViewModel> GetDetailsByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AdFormModel> GetFormModelByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsSellerWithIdAsync(int adId, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
