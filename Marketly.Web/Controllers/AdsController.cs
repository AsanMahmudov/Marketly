using Marketly.Core.Interfaces;
using Marketly.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Marketly.Web.Controllers
{
    public class AdsController : Controller
    {
        private readonly IAdService adService;
        private readonly ICategoryService categoryService;
        private readonly IUserService userService;

        public AdsController(IAdService _adService, ICategoryService _categoryService, IUserService _userService)
        {
            adService = _adService;
            categoryService = _categoryService;
            userService = _userService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery] AdQueryModel query)
        {
            var serviceModel = await adService.AllAsync(query.SelectedCategory, query.SearchTerm, query.CurrentPage == 0 ? 1 : query.CurrentPage, 6);
            query.TotalAds = serviceModel.TotalAds;
            query.Ads = serviceModel.Ads;
            query.TotalPages = serviceModel.TotalPages;
            query.Categories = await categoryService.AllCategoriesAsync();
            return View(query);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            if (!await adService.ExistsAsync(id)) return NotFound();
            return View(await adService.GetDetailsByIdAsync(id));
        }

        [Authorize]
        public async Task<IActionResult> Create() => View(new AdFormModel { Categories = await categoryService.AllCategoriesAsync() });

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await categoryService.AllCategoriesAsync();
                return View(model);
            }
            await adService.CreateAsync(model, User.FindFirstValue(ClaimTypes.NameIdentifier));
            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            if (!await adService.IsSellerWithIdAsync(id, User.FindFirstValue(ClaimTypes.NameIdentifier))) return Unauthorized();
            var model = await adService.GetFormModelByIdAsync(id);
            model.Categories = await categoryService.AllCategoriesAsync();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdFormModel model)
        {
            if (!await adService.IsSellerWithIdAsync(id, User.FindFirstValue(ClaimTypes.NameIdentifier)))
                return Unauthorized();

            if (!ModelState.IsValid)
            {
                model.Categories = await categoryService.AllCategoriesAsync();
                return View(model);
            }

            await adService.EditAsync(id, model);
            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        public async Task<IActionResult> Watchlist() => View(await userService.GetFavoriteAdsAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)));

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToWatchlist(int id)
        {
            await userService.AddToFavoritesAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), id);
            return RedirectToAction(nameof(Watchlist));
        }
    }
}