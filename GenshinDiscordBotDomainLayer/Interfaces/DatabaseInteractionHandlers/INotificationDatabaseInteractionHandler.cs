using GenshinDiscordBotDomainLayer.DomainModels.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.Interfaces.DatabaseInteractionHandlers
{
    public interface INotificationDatabaseInteractionHandler
    {
        public Task<IList<NotificationJob>> GetNotificationJobsAsync();
        public Task UpdateNotificationJobAsync(NotificationJob job);
    }
}
