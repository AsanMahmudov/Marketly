using Marketly.Core.Common;
using Marketly.Core.Interfaces;
using Marketly.Core.Models;
using Marketly.Core.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Marketly.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly IRepository repository;
        public MessageService(IRepository _repository) => repository = _repository;

        public async Task<IEnumerable<MessageInboxViewModel>> GetUserMessagesAsync(string userId)
        {
            return await repository.All<Message>()
                .Where(m => m.ReceiverId == userId || m.SenderId == userId)
                .OrderByDescending(m => m.SentOn)
                .Select(m => new MessageInboxViewModel
                {
                    Id = m.Id,
                    SenderName = m.SenderId == userId ? "Me" : "User", 
                    SenderId = m.SenderId,
                    Content = m.Content,
                    SentDate = m.SentOn,
                    RelatedAdId = m.AdId
                })
                .ToListAsync();
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
            var anchor = await repository.All<Message>()
                .FirstOrDefaultAsync(m => m.Id == messageId);

            if (anchor == null) return Enumerable.Empty<MessageInboxViewModel>();

            return await repository.All<Message>()
                .Where(m => m.AdId == anchor.AdId &&
                      ((m.SenderId == anchor.SenderId && m.ReceiverId == anchor.ReceiverId) ||
                       (m.SenderId == anchor.ReceiverId && m.ReceiverId == anchor.SenderId)))
                .OrderBy(m => m.SentOn) // Oldest at top, newest at bottom
                .Select(m => new MessageInboxViewModel
                {
                    Id = m.Id,
                    Content = m.Content,
                    SentDate = m.SentOn,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    SenderName = m.SenderId == userId ? "Me" : "User",
                    RelatedAdId = m.AdId
                })
                .ToListAsync();
        }
    }
}