using Marketly.Core.Interfaces;
using Marketly.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Marketly.Web.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly IMessageService messageService;
        public MessagesController(IMessageService _messageService) => messageService = _messageService;

        public async Task<IActionResult> Inbox()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var messages = await messageService.GetUserMessagesAsync(userId);
            return View(messages);
        }

        public IActionResult Create(string recipientId, int adId)
        {
            return View(new MessageFormModel { RecipientId = recipientId, RelatedAdId = adId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MessageFormModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (senderId == model.RecipientId) return BadRequest("Cannot message yourself.");

            await messageService.SendMessageAsync(model, senderId);
            return RedirectToAction(nameof(Inbox));
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var thread = await messageService.GetConversationAsync(id, userId);

            if (thread == null || !thread.Any()) return NotFound();
            return View(thread);
        }
    }
}