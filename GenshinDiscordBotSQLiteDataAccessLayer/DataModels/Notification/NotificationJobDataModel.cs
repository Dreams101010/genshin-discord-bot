using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels.Notification;

namespace GenshinDiscordBotSQLiteDataAccessLayer.DataModels.Notification
{
    public class NotificationJobDataModel
    {
        public int JobId { get; set; }
        public string JobKind { get; set; }
        public string JobData { get; set; }
        public ulong SuccessEndpointUserId { get; set; }
        public ulong SuccessEndpointGuildId { get; set; }
        public ulong SuccessEndpointChannelId { get; set; }
        public ulong ErrorEndpointUserId { get; set; }
        public ulong ErrorEndpointGuildId { get; set; }
        public ulong ErrorEndpointChannelId { get; set; }

        public NotificationJobDataModel(NotificationJob job)
        {
            JobId = job.Id;
            JobKind = job.Kind.ToString();
            JobData = job.DataJson;
            SuccessEndpointUserId = job.SuccessEndpoint.UserDiscordId;
            SuccessEndpointGuildId = job.SuccessEndpoint.GuildId;
            SuccessEndpointChannelId = job.SuccessEndpoint.ChannelId;
            ErrorEndpointUserId = job.ErrorEndpoint.UserDiscordId;
            ErrorEndpointGuildId = job.ErrorEndpoint.GuildId;
            ErrorEndpointChannelId = job.ErrorEndpoint.ChannelId;
        }

        public NotificationJob ToNotificationJobDomain()
        {
            return new NotificationJob()
            {
                Id = JobId,
                Kind = (NotificationJobKind)Enum.Parse(typeof(NotificationJobKind), JobKind),
                DataJson = JobData,
                SuccessEndpoint = new(SuccessEndpointUserId,
                    SuccessEndpointGuildId, SuccessEndpointChannelId),
                ErrorEndpoint = new(ErrorEndpointUserId,
                    ErrorEndpointGuildId, ErrorEndpointChannelId),
            };
        }
    }
}
