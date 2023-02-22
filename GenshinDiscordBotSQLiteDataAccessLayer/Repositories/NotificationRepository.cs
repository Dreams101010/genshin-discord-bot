using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels.Notification;
using GenshinDiscordBotDomainLayer.Interfaces;
using Dapper;
using Microsoft.Data.Sqlite;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotSQLiteDataAccessLayer.DataModels.Notification;

namespace GenshinDiscordBotSQLiteDataAccessLayer.Repositories
{
    internal class NotificationRepository : INotificationRepository
    {
        public SqliteConnection Connection { get; }

        public NotificationRepository(SqliteConnection connection)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }
        public async Task<IList<NotificationJob>> GetNotificationJobsAsync()
        {
            string sql = @"
                SELECT 
                notif_jobs.id JobId,
                notif_jobs.kind JobKind,
                notif_jobs.data JobData,
                success_endpoints.user_discord_id SuccessEndpointUserId,
                success_endpoints.guild_id SuccessEndpointGuildId,
                success_endpoints.channel_id SuccessEndpointChannelId,
                error_endpoints.user_discord_id ErrorEndpointUserId,
                error_endpoints.guild_id ErrorEndpointGuildId,
                error_endpoints.channel_id ErrorEndpointChannelId
                FROM notif_jobs 
                JOIN notif_endpoints success_endpoints ON notif_jobs.success_endpoint_id = success_endpoints.id
                JOIN notif_endpoints error_endpoints ON notif_jobs.error_endpoint_id = error_endpoints.id;
            ";
            return (await Connection.QueryAsync<NotificationJobDataModel>
                (sql)).Select(x => x.ToNotificationJobDomain()).ToList();
        }

        public async Task UpdateNotificationJobAsync(NotificationJob job)
        {
            string updateSql = @"
                UPDATE notif_jobs SET data = @DataJson WHERE id = @Id;
            ";
            int affectedByUpdate = await Connection.ExecuteAsync(updateSql, job);
            if (affectedByUpdate == 0)
            {
                throw new Exception("Database error while updating rows: notification job not found");
            }
        }
    }
}
