using Marketly.Core.Interfaces;
using Marketly.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketly.Core.Services
{
    public class MessageService : IMessageService
    {
        public Task<IEnumerable<MessageInboxViewModel>> GetUserMessagesAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task SendMessageAsync(MessageFormModel model, string senderId)
        {
            throw new NotImplementedException();
        }
    }
}
