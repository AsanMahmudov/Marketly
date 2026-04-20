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

        public MessagesController(IMessageService _messageService)
            => messageService = _messageService;

        /// <summary>
        /// Displays the user's conversation history.
        /// </summary>
        public async Task<IActionResult> Inbox()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var messages = await messageService.GetUserMessagesAsync(userId);
            return View(messages);
        }

        /// <summary>
        /// Loads the message creation form. 
        /// Matches the 'Message Seller' button on the Ad Details page.
        /// </summary>
        public IActionResult Create(string recipientId, int adId)
        {
            var model = new MessageFormModel
            {
                RecipientId = recipientId,
                RelatedAdId = adId
            };

            return View(model);
        }

        /// <summary>
        /// Processes the message submission.
        /// Matches the form in Create.cshtml.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MessageFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Prevent users from messaging themselves
            if (senderId == model.RecipientId)
            {
                return BadRequest("You cannot send a message to yourself.");
            }

            await messageService.SendMessageAsync(model, senderId);

            return RedirectToAction(nameof(Inbox));
        }


        public async Task<IActionResult> Details(int id)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<MessageInboxViewModel> thread = await messageService.GetConversationAsync(id, userId);

            if (!thread.Any()) return NotFound();

            return View(thread); 
        }
    }
}