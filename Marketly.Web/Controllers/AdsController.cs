using Marketly.Core.Interfaces;
using Marketly.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
public class AdsController : Controller
{
    private readonly IAdService adService;
    private readonly ICategoryService categoryService;

    public AdsController(IAdService _adService, ICategoryService _categoryService)
    {
        adService = _adService;
        categoryService = _categoryService;
    }

    [AllowAnonymous]
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

    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
        if (!await adService.ExistsAsync(id)) return NotFound();
        return View(await adService.GetDetailsByIdAsync(id));
    }

    public async Task<IActionResult> Create()
        => View(new AdFormModel { Categories = await categoryService.AllCategoriesAsync() });

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

    public async Task<IActionResult> Edit(int id)
    {
        if (!await adService.IsSellerWithIdAsync(id, User.FindFirstValue(ClaimTypes.NameIdentifier))) return Unauthorized();
        var model = await adService.GetFormModelByIdAsync(id);
        model.Categories = await categoryService.AllCategoriesAsync();
        return View(model); 
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AdFormModel model)
    {
        if (!await adService.IsSellerWithIdAsync(id, User.FindFirstValue(ClaimTypes.NameIdentifier))) return Unauthorized();
        if (!ModelState.IsValid)
        {
            model.Categories = await categoryService.AllCategoriesAsync();
            return View(model);
        }
        await adService.EditAsync(id, model);
        return RedirectToAction(nameof(Details), new { id });
    }

    public async Task<IActionResult> MyAds()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var ads = await adService.GetAdsByUserIdAsync(userId);

        return View(ads);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!await adService.IsSellerWithIdAsync(id, userId))
        {
            return Unauthorized();
        }

        await adService.DeleteAsync(id);
        return RedirectToAction(nameof(MyAds));
    }
}