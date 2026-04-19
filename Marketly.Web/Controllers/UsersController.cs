using Marketly.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
public class UsersController : Controller
{
    private readonly IUserService userService;
    public UsersController(IUserService _userService) => userService = _userService;

    [AllowAnonymous]
    public async Task<IActionResult> Profile(string id)
    {
        var model = await userService.GetUserProfileAsync(id);
        if (model == null) return NotFound();
        return View(model);
    }

    public async Task<IActionResult> Watchlist()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var ads = await userService.GetFavoriteAdsAsync(userId);
        return View(ads); 
    }

    [HttpPost]
    public async Task<IActionResult> AddToWatchlist(int id)
    {
        await userService.AddToFavoritesAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), id);
        return RedirectToAction(nameof(Watchlist));
    }
}