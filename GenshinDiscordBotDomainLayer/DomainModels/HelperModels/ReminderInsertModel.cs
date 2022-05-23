using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.DomainModels.HelperModels
{
    public struct ReminderInsertModel
    {
        public ulong UserDiscordId { get; set; }
        public ulong GuildId { get; set; }
        public ulong ChannelId { get; set; }
        public ulong Interval { get; set; }
        public ulong ReminderTime { get; set; }
        public string CategoryName { get; set; }
        public string Message { get; set; }
        public bool Recurrent { get; set; }
    }
}
