using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;

namespace GenshinDiscordBotSQLiteDataAccessLayer.DataModels
{
    public struct ResinTrackingInfoDataModel
    {
        public ulong UserDiscordId { get; set; }
        public string StartTime { get; set; } // stored in string form in SQLite
        public int StartCount { get; set; }

        public ResinTrackingInfoDataModel(ResinTrackingInfo resinTrackingInfo)
        {
            UserDiscordId = resinTrackingInfo.UserDiscordId;
            StartTime = resinTrackingInfo.StartTime.ToString("s");
            StartCount = resinTrackingInfo.StartCount;
        }

        public ResinTrackingInfo ToResinTrackingInfo()
        {
            return new ResinTrackingInfo
            {
                UserDiscordId = this.UserDiscordId,
                StartTime = DateTime.Parse(this.StartTime),
                StartCount = this.StartCount
            };
        }
    }
}
