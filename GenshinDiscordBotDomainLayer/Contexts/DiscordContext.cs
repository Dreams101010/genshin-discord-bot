using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.Contexts
{
    public class DiscordContext
    {
        public ulong GuildId { get; private set; }
        public ulong ChannelId { get; private set; }
        public ulong UserId { get; private set; }
        public string UserName { get; private set; }

        public void SetDiscordContextData(ulong guildId, ulong channelId, ulong userId, string userName)
        {
            GuildId = guildId;
            ChannelId = channelId;
            UserId = userId;
            UserName = userName;
        }
    }
}
