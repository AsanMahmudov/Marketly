using Marketly.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Area("Admin")]
[Authorize(Roles = "Administrator")]
public class CategoriesController : Controller
{
    private readonly ICategoryService categoryService;
    public CategoriesController(ICategoryService _categoryService) => categoryService = _categoryService;

    public async Task<IActionResult> Manage()
        => View(await categoryService.GetAllForAdminAsync()); // View #13: Category List

    public IActionResult Create() => View(); // View #14: Add Category

    [HttpPost]
    public async Task<IActionResult> Create(string name)
    {
        await categoryService.CreateAsync(name);
        return RedirectToAction(nameof(Manage));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await categoryService.DeleteAsync(id); // View #15: Delete Confirmation (or partial)
        return RedirectToAction(nameof(Manage));
    }
}