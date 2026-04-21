using Marketly.Core.Interfaces;
using Marketly.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Marketly.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAdService adService;
        private readonly ICategoryService categoryService;

        public HomeController(IAdService _adService, ICategoryService _categoryService)
        {
            adService = _adService;
            categoryService = _categoryService;
        }

        public async Task<IActionResult> All([FromQuery] AdQueryModel query)
        {
            var serviceModel = await adService.AllAsync(
                query.SelectedCategory,
                query.SearchTerm,
                query.CurrentPage == 0 ? 1 : query.CurrentPage,
                8);

            query.TotalAds = serviceModel.TotalAds;
            query.Ads = serviceModel.Ads;
            query.TotalPages = serviceModel.TotalPages;
            query.Categories = await categoryService.AllCategoriesAsync();

            return View(query);
        }

        public IActionResult About() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode)
        {
            if (statusCode == 404) return View("NotFound");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}