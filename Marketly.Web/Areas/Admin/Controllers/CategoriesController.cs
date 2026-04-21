using Marketly.Core.Interfaces;
using Marketly.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketly.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService categoryService;
        public CategoriesController(ICategoryService _categoryService) => categoryService = _categoryService;

        public async Task<IActionResult> Manage()
            => View(await categoryService.GetAllForAdminAsync());

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryFormModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await categoryService.CreateAsync(model);
            TempData["SuccessMessage"] = "Category created!";
            return RedirectToAction(nameof(Manage));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            bool success = await categoryService.DeleteAsync(id);
            if (!success) TempData["ErrorMessage"] = "Category is not empty!";
            else TempData["SuccessMessage"] = "Category deleted.";

            return RedirectToAction(nameof(Manage));
        }
    }
}