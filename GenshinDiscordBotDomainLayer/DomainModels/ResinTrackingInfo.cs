using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.DomainModels
{
    public struct ResinTrackingInfo
    {
        public ulong UserDiscordId { get; set; }
        public DateTime StartTime { get; set; }
        public int StartCount { get; set; }
    }
}
