using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.DomainModels.Notification
{
    public enum NotificationJobKind { GenshinPromocodes, HonkaiImpact3rdPromocodes, HonkaiStarRailPromocodes }
    public class NotificationJob
    {
        public int Id { get; init; }
        public NotificationJobKind Kind { get; init; }
        public string DataJson { get; set; }
        public NotificationEndpoint SuccessEndpoint { get; init; }
        public NotificationEndpoint ErrorEndpoint { get; init; }
    }
}
