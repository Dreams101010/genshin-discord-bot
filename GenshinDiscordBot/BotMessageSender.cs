using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.DomainModels;
using Discord.WebSocket;

namespace GenshinDiscordBotUI
{
    public class BotMessageSender : IBotMessageSender
    {
        public DiscordSocketClient Client { get; }

        public BotMessageSender(DiscordSocketClient client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task SendMessageAsync(MessageContext messageContext)
        {
            var guild = Client.GetGuild(messageContext.GuildId);
            var channel = guild.GetTextChannel(messageContext.ChannelId);
            var user = await Client.GetUserAsync(messageContext.DiscordUserId);
            if (user != null)
            {
                await channel.SendMessageAsync($"{user.Mention} {messageContext.Message}");
            }
        }
    }
}
