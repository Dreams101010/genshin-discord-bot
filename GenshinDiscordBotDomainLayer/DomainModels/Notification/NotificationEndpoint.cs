using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.DomainModels.Notification
{
    public struct NotificationEndpoint
    {
        public ulong UserDiscordId { get; init; }
        public ulong GuildId { get; init; }
        public ulong ChannelId { get; init; }

        public NotificationEndpoint(ulong userDiscordId, ulong guildId, ulong channelId)
        {
            if (userDiscordId == 0 && (guildId == 0 || channelId == 0))
            {
                throw new ArgumentException("Invalid notification endopoint: cannot create endpoint" +
                    " with no user and no channel");
            }
            UserDiscordId = userDiscordId;
            GuildId = guildId;
            ChannelId = channelId;
        }

        public bool IsDm()
        {
            return (UserDiscordId != 0);
        }

        public bool IsChannel()
        {
            return (UserDiscordId == 0);
        }
    }
}
