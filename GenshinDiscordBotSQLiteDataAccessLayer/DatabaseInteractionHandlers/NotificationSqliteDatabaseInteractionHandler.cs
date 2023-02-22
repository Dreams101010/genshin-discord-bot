using GenshinDiscordBotDomainLayer.DomainModels.Notification;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.Interfaces.DatabaseInteractionHandlers;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotSQLiteDataAccessLayer.DatabaseInteractionHandlers
{
    public class NotificationSqliteDatabaseInteractionHandler 
        : SqliteDatabaseInteractionHandler, INotificationDatabaseInteractionHandler
    {
        private INotificationRepository NotificationRepository { get; }

        public NotificationSqliteDatabaseInteractionHandler(
            INotificationRepository notificationRepository, SqliteConnection connection)
            : base(connection)
        {
            NotificationRepository = notificationRepository 
                ?? throw new ArgumentNullException(nameof(notificationRepository));
        }
        public async Task<IList<NotificationJob>> GetNotificationJobsAsync()
        {
            return await ExecuteInTransactionAsync(
                async () => await GetNotificationJobsBodyAsync());
        }

        private async Task<IList<NotificationJob>> GetNotificationJobsBodyAsync()
        {
            return await NotificationRepository.GetNotificationJobsAsync();
        }

        public async Task UpdateNotificationJobAsync(NotificationJob job)
        {
            await ExecuteInTransactionAsync(
                async () => await UpdateNotificationJobBodyAsync(job));
        }

        private async Task UpdateNotificationJobBodyAsync(NotificationJob job)
        {
            await NotificationRepository.UpdateNotificationJobAsync(job);
        }
    }
}
