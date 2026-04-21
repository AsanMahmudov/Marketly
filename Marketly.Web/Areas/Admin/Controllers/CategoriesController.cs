using Marketly.Core.Interfaces;
using Marketly.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Area("Admin")]
[Authorize(Roles = "Administrator")]
public class CategoriesController : Controller
{
    private readonly ICategoryService categoryService;
    public CategoriesController(ICategoryService _categoryService)
        => categoryService = _categoryService;

    public async Task<IActionResult> Manage()
        => View(await categoryService.GetAllForAdminAsync());

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryFormModel model)
    {
        if (!ModelState.IsValid) return View(model);

        await categoryService.CreateAsync(model);
        TempData["SuccessMessage"] = "Category created successfully!";
        return RedirectToAction(nameof(Manage));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        bool success = await categoryService.DeleteAsync(id);

        if (!success)
        {
            TempData["ErrorMessage"] = "Cannot delete category while it contains advertisements.";
        }
        else
        {
            TempData["SuccessMessage"] = "Category removed successfully.";
        }

        return RedirectToAction(nameof(Manage));
    }
}