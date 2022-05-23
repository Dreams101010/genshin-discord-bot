using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.DomainModels.HelperModels
{
    public struct DiscordMessageContext
    {
        public ulong UserDiscordId { get; set; }
        public ulong GuildId { get; set; }
        public ulong ChannelId { get; set; }
    }
}
