using Marketly.Core.Interfaces;
using Marketly.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Marketly.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAdService adService;

        public HomeController(IAdService _adService)
        {
            adService = _adService;
        }

        public async Task<IActionResult> Index()
        {
            // Landing page shows the 3 latest listings
            var model = await adService.GetLatestAsync(3);
            return View(model);
        }

        public IActionResult About() => View(); // View #2: Project Concept

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode)
        {
            if (statusCode == 404) return View("NotFound"); // View #3: 404 Page
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}