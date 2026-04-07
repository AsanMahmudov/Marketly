using Marketly.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketly.Core.Interfaces
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageInboxViewModel>> GetUserMessagesAsync(string userId);
        Task SendMessageAsync(MessageFormModel model, string senderId);
    }
}
