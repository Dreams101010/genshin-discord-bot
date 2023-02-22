using GenshinDiscordBotDomainLayer.DomainModels.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.Interfaces
{
    public interface INotifier
    {
        public Task<bool> Notify(string message, NotificationEndpoint endpoint);
    }
}
