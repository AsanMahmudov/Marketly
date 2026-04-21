using Marketly.Core.Common;
using Marketly.Core.Interfaces;
using Marketly.Core.Models;
using Marketly.Core.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Marketly.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly IApplicationRepository repository;
        public MessageService(IApplicationRepository _repository) => repository = _repository;

        public async Task<IEnumerable<MessageInboxViewModel>> GetUserMessagesAsync(string userId)
        {
            // 1. Get all messages involving the user
            var allMessages = await repository.All<Message>()
                .Where(m => m.ReceiverId == userId || m.SenderId == userId)
                .OrderByDescending(m => m.SentOn)
                .ToListAsync();

            // 2. Group them by the "Conversation Key" (The Ad + The two people talking)
            return allMessages
                .GroupBy(m => new
                {
                    m.AdId,
                    // Sort IDs to ensure (UserA -> UserB) and (UserB -> UserA) are the same group
                    ParticipantKey = string.Join("-", new[] { m.SenderId, m.ReceiverId }.OrderBy(s => s))
                })
                .Select(g => {
                    var latest = g.First(); // The most recent message in this conversation
                    return new MessageInboxViewModel
                    {
                        Id = latest.Id,
                        SenderId = latest.SenderId,
                        ReceiverId = latest.ReceiverId,
                        SenderName = latest.SenderId == userId ? "Me" : "User",
                        Content = latest.Content,
                        SentDate = latest.SentOn,
                        RelatedAdId = latest.AdId
                    };
                })
                .ToList();
        }

        public async Task SendMessageAsync(MessageFormModel model, string senderId)
        {
            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = model.RecipientId,
                Content = model.Content,       
                AdId = model.RelatedAdId ?? 0, 
                SentOn = DateTime.UtcNow
            };

            await repository.AddAsync(message);
            await repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<MessageInboxViewModel>> GetConversationAsync(int messageId, string userId)
        {
            // 1. Find the anchor message first to get AdId and participants
            var anchor = await repository.All<Message>()
                .Include(m => m.Ad)
                .ThenInclude(a => a.Images)
                .FirstOrDefaultAsync(m => m.Id == messageId);

            if (anchor == null) return Enumerable.Empty<MessageInboxViewModel>();

            // 2. Prepare the Ad data beforehand (Client-side)
            string adTitle = anchor.Ad.Title;
            string adImageUrl = anchor.Ad.Images.OrderBy(i => i.Id).Select(i => i.Url).FirstOrDefault()
                                ?? "/img/no-image.png";

            // 3. Fetch the conversation history
            var thread = await repository.All<Message>()
                .Where(m => m.AdId == anchor.AdId &&
                      ((m.SenderId == anchor.SenderId && m.ReceiverId == anchor.ReceiverId) ||
                       (m.SenderId == anchor.ReceiverId && m.ReceiverId == anchor.SenderId)))
                .OrderBy(m => m.SentOn)
                .ToListAsync(); // Pull to memory first to make mapping simple

            // 4. Map to ViewModels
            return thread.Select(m => new MessageInboxViewModel
            {
                Id = m.Id,
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                SenderName = m.SenderId == userId ? "Me" : "User",
                Content = m.Content,
                SentDate = m.SentOn,
                RelatedAdId = m.AdId,
                AdTitle = adTitle,
                AdImageUrl = adImageUrl
            });
        }
    }
}