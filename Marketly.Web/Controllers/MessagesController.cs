using Marketly.Core.Interfaces;
using Marketly.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
public class MessagesController : Controller
{
    private readonly IMessageService messageService;
    public MessagesController(IMessageService _messageService) => messageService = _messageService;

    public async Task<IActionResult> Inbox()
        => View(await messageService.GetUserMessagesAsync(User.FindFirstValue(ClaimTypes.NameIdentifier))); 

    public IActionResult Send(string recipientId, int adId)
        => View(new MessageFormModel { RecipientId = recipientId, RelatedAdId = adId }); 

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Send(MessageFormModel model)
    {
        if (!ModelState.IsValid) return View(model);
        await messageService.SendMessageAsync(model, User.FindFirstValue(ClaimTypes.NameIdentifier));
        return RedirectToAction(nameof(Inbox));
    }
}